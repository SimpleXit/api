using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using SimpleX.Helpers.Interfaces;
using SimpleX.Helpers.Settings;
using SimpleX.Web.Api.Root;
using SimpleX.Web.Api.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleX.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerFactory = new NLogLoggerFactory();
        }

        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers()
                    .AddXmlSerializerFormatters()
                    .AddJsonOptions(jsonOptions =>
                     {
                         jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                     });

            services.AddMvc(config =>
            {
                // Add XML Content Negotiation
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = false;
            })
                .AddXmlSerializerFormatters()
                .AddFluentValidation(fv =>
                    {
                        fv.RegisterValidatorsFromAssembly(Assembly.Load("SimpleX.Domain.Validation"));
                        fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    });  

            //services.AddCors();

            services.AddSwagger();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // configure jwt authentication
            var securitySettings = Configuration.GetSection("SecuritySettings").Get<SecuritySettings>();
            services.AddJwtAuthentication(securitySettings.SecurityKey);

            // API services:
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IFileService, FileService>();

            // SimpleX:
            services.ConfigureServices(Configuration, LoggerFactory);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpleX.Web.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
            logger.LogInformation("Listening on: " + string.Join(", ", serverAddressesFeature.Addresses));
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, string securityKey)
        {
            var key = Encoding.ASCII.GetBytes(securityKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                //x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
