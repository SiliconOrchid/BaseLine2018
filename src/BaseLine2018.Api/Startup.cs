
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using AutoMapper;
using AspNetCoreRateLimit;

using BaseLine2018.Api.StartupExtensions;
using BaseLine2018.Common.Models.Configuration;
using BaseLine2018.Data.Context;
using BaseLine2018.Data.StartupExtensions;
using BaseLine2018.Email.StartupExtensions;
using BaseLine2018.Service.StartupExtensions;


namespace BaseLine2018.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }



        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ------ appsetting/secrets configurations -----------
            services.Configure<EmailSettingsConfig>(Configuration.GetSection("EmailSettingsConfig"));
            services.Configure<FeatureSwitchesConfig>(Configuration.GetSection("FeatureSwitchesConfig"));
            services.Configure<MemoryCachingConfig>(Configuration.GetSection("MemoryCachingConfig"));
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // ------ Other Framework Services ------
            services.AddOptions();
            services.AddAutoMapper();
            services.AddMemoryCache();
            services.ConfigureCors(CorsInitialisationExtension.GetCorsEndpointsFromConfiguration(Configuration.GetSection("Endpoints")));
            services.AddMvc();


            // --------- IoC Registration -------------
            services.AddDataServices();
            services.AddServiceServices();
            services.AddApiServices();
            services.AddEmailServices();


            // ------ Database ------
            var connection = Configuration.GetConnectionString("DefaultSql");
            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(connection);
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            // --------- Apply Startup Extensions -------------
            LoggingStartupExtension.SetLogging(app, loggerFactory, Configuration);
            GeneralStartupExtension.SeedDatabase(app, Configuration);
            GeneralStartupExtension.SetGlobalisation(app);


            // --------- Development specific -------------
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Debug);
            }


            // --------- Use services -------------
            //app.UseCors(env.GetCorsAllowances());
            app.UseIpRateLimiting();
            //app.UseAuthentication(); //noting that "UseAuthentication()"  is a replacement for the deprecated "UseIdentity()" method
            app.UseMvc();

        }
    }
}
