using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using SimpleX.Core.Crypto;
using SimpleX.Data.Context;
using SimpleX.Domain.Services.Database;
using SimpleX.Domain.Services.Helpers;
using SimpleX.Domain.Tasks.Helpers;
using SimpleX.Helpers.Mail;
using SimpleX.Helpers.Settings;
using System;
using System.Reflection;

namespace SimpleX.Web.Api.Root
{
    public static class CompositionRoot
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config, ILoggerFactory loggerFactory)
        {
            ///Data
            services.AddSimpleDbContext(config, loggerFactory);

            ///Services
            services.AddSimpleServices();

            ///Tasks
            services.AddSimpleTasks();

            ///Settings
            services.AddSettings<SecuritySettings>(config.GetSection("SecuritySettings"));
            services.AddSettings<MailSettings>(config.GetSection("MailSettings"));

            ///Helpers
            services.AddTransient<ICryptoService, CryptoService>();
            services.AddTransient<IMailService, MailService>();

            services.AddAutoMapper(c => c.AddCollectionMappers(), Assembly.Load("SimpleX.Domain.Mappings"));

            return services;
        }

        public static TConfig AddSettings<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        public static IServiceCollection AddSimpleTasks(this IServiceCollection services)
        {
            try
            {        
                //MethodInfo methodInfo = typeof(ServiceCollectionServiceExtensions).GetMethods()
                //                            .FirstOrDefault(p => p.Name == nameof(ServiceCollectionServiceExtensions.AddScoped));

                foreach (Type task in TaskFinder.GetAllTasks())
                {
                    services.AddTransient(task);

                    //var genericMethod_AddScoped = methodInfo.MakeGenericMethod(task);
                    //_ = genericMethod_AddScoped.Invoke(obj: null, parameters: new object[] { services }); 
                }

                services.AddHostedService<TaskRunner>();

                return services;
            }
            catch
            {
                throw;
            }

        }

        public static IServiceCollection AddSimpleServices(this IServiceCollection services)
        {
            var assembliesToScan = new[]
               {
                    ServiceFinder.GetServiceAssembly()
               };

            var result = services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                                 .IgnoreThisInterface<IMigrationService>()
                                 .IgnoreThisInterface<ISeedingService>()
                                 .AsPublicImplementedInterfaces(ServiceLifetime.Transient);

#if DEBUG
            services.AddTransient<IMigrationService, DebugMigrationService>();
            services.AddTransient<ISeedingService, DebugSeedingService>();
#else
            services.AddTransient<IMigrationService, MigrationService>();
            services.AddTransient<ISeedingService, SeedingService>();
#endif


            ///Helpers
            services.AddTransient<DefaultValueSetter>();

            return services;
        }

        public static IServiceCollection AddSimpleDbContext(this IServiceCollection services, IConfiguration config, ILoggerFactory loggerFactory)
        {
            DbSettings settings = config.GetSection("DbSettings").Get<DbSettings>();
            services.AddDbContextFactory<SimpleContext>(f =>
            {
                f.UseSqlServer(settings.ConnectionString);
                f.EnableDetailedErrors(settings.EnableLogging);
                f.EnableSensitiveDataLogging(settings.EnableSensitiveLogging);
                f.UseLoggerFactory(loggerFactory);
            });

            return services;
        }
    }
}
