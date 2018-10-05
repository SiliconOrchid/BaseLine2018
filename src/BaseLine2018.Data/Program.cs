using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using BaseLine2018.Common.Models.Domain.Identity;
using BaseLine2018.Data.Context;

namespace BaseLine2018.Data
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            // --- environment ---
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";



            // --- configuration ---
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();





            // --- services ---
            var services = new ServiceCollection();

            var connection = Configuration.GetConnectionString("DefaultSql");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connection);

                // Register the entity sets needed by OpenIddict. Note: use the generic overload if you need to replace the default OpenIddict entities.

            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


        }
    }
}
