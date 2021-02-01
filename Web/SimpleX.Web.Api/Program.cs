using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using SimpleX.Domain.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Initializing application...");

                var host = CreateHostBuilder(args).Build();

                try
                {
                    logger.Info("Migrating database...");

                    using (var scope = host.Services.CreateScope())
                    {
                        try
                        {
                            using (IMigrationService migrater = scope.ServiceProvider.GetRequiredService<IMigrationService>())
                            {
                                migrater.Migrate();
                            }
                            using (ISeedingService seeder = scope.ServiceProvider.GetRequiredService<ISeedingService>())
                            {
                                seeder.SeedDb();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "An error occurred while migrating the database.");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while building the host.");
                    throw;
                }

                logger.Warn($"Starting application...");
                host.Run();
                Environment.Exit(0);
                logger.Info($"Closing application...");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception.");
                Environment.Exit(1);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                .UseNLog()
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
