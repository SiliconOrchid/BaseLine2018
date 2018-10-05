using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

using BaseLine2018.Common.Logging;
using BaseLine2018.Data.Context;
using BaseLine2018.Data.Extension;
using BaseLine2018.Common.Models.Domain.Identity;


namespace BaseLine2018.Api.StartupExtensions
{
    /// <summary>
    /// Startup Extensions ... General-purpose class, intended to be a place where various multi-line items,
    /// that would otherwise be found in the startup.cs, can be tidied away to.
    /// </summary>
    public static class GeneralStartupExtension

    {
        public static void SetGlobalisation(IApplicationBuilder app)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.1
            var supportedCultures = new[]
            {
                new CultureInfo("en-GB"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }


        public static void SeedDatabase(IApplicationBuilder app, IConfiguration configuration)
        {
            //TODO : JM16Jul18 (I would prefer to work with a strongly typed configuration object here (as you would elsewhere in the application, when you inject a configuration object), but couldn't see a way to do this in startup
            if (configuration["FeatureSwitchesConfig:EnableDatabaseAutoSeeding"].ToLower() == "true")
            {
                app.EnsureDatabaseIsSeeded(false).GetAwaiter().GetResult();
            }
        }




        private static async Task<int> EnsureDatabaseIsSeeded(this IApplicationBuilder applicationBuilder, bool autoMigrateDatabase)
        {
            try
            {
                using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {

                    if (serviceScope == null)
                    {
                        throw new Exception("[ConfigureHttpPipelineExtension.EnsureDatabaseIsSeeded]  Could not resolve 'serviceScope' from applicationBuilder");
                    }


                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                    if (context == null)
                    {
                        throw new Exception("[ConfigureHttpPipelineExtension.EnsureDatabaseIsSeeded]  Could not resolve an ApplicationContext from call to 'GetService' - have you correctly registered database in the projects startup.cs ?");
                    }

                    //if(autoMigrateDatabase)
                    //{
                    //    //context.Database.Migrate();
                    //}
                    return await context.EnsureSeedData();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[ConfigureHttpPipelineExtension.EnsureDatabaseIsSeeded] : Unexpected exception : ", ex);
                throw;
            }

        }
    }
}
