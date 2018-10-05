using System;
using System.Linq;
using BaseLine2018.Common.Exceptions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseLine2018.Api.StartupExtensions
{
    public static class CorsInitialisationExtension
    {
        internal const string ALLOW_ALL = "AllowAll";
        internal const string ALLOW_AZURE = "AllowAzureSites";

        internal static string GetCorsAllowances(this IHostingEnvironment env) => env.IsDevelopment() ? ALLOW_ALL : ALLOW_AZURE;

        internal static IServiceCollection ConfigureCors(this IServiceCollection services, string hostingOrigin)
        {
            string[] origins = hostingOrigin.Split(',');

            return ConfigureCors(services, origins);
        }

        internal static IServiceCollection ConfigureCors(this IServiceCollection services, string[] hostingOrigins)
        {
            if (hostingOrigins == null)
                throw new ArgumentNullException("[CorsInitialisationExtension.ConfigureCors]  No hosting origin endpoints for CORS configuration were specified. Please check app settings.");

            CorsPolicyBuilder corsPolicyBuilder = GetBasicCorsPolicyBuilder();
            corsPolicyBuilder.WithOrigins(hostingOrigins);
            CorsPolicy allowAzurePolicy = corsPolicyBuilder.Build();

            corsPolicyBuilder = GetBasicCorsPolicyBuilder();
            corsPolicyBuilder.AllowAnyOrigin();
            CorsPolicy allowAllPolicy = corsPolicyBuilder.Build();

            services.AddCors(options =>
            {
                options.AddPolicy(ALLOW_ALL, allowAllPolicy);
                options.AddPolicy(ALLOW_AZURE, allowAzurePolicy);
            });

            return services;
        }

        internal static CorsPolicyBuilder GetBasicCorsPolicyBuilder()
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowCredentials();
            return corsBuilder;
        }

        public static string[] GetCorsEndpointsFromConfiguration(IConfigurationSection endpointsSection)
        {
            var corsUriConfig = endpointsSection.GetSection("WebEndpoints");
            if (corsUriConfig == null || !corsUriConfig.Exists())
                throw new AppSettingsException("[CorsInitialisationExtension.CorsEndpoints]  No hosting origin endpoints for CORS configuration were specified. Please check app settings.", corsUriConfig?.Path);

            var corsEndpoints = corsUriConfig.Get<string[]>();
            if (corsEndpoints != null && corsEndpoints.All(string.IsNullOrWhiteSpace))
                throw new AppSettingsException("[CorsInitialisationExtension.CorsEndpoints] No valid hosting origin endpoints for CORS configuration were specified. Please check app settings.", corsUriConfig?.Path);
            return corsEndpoints;
        }

    }
}