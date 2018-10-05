using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BaseLine2018.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Required to support command line tooling (i.e. to be able to run "dotnet ef migrations add initial" etc)
        /// </summary>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";


            DbContextOptionsBuilder<ApplicationDbContext> builder;
            IConfigurationRoot configuration;


            if (isDevelopment)
            {
                //only use UserSecrets (for connection string) in development
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<ApplicationDbContext>()
                    .Build();
                builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            }
            else
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            }

            var connectionString = configuration.GetConnectionString("DefaultSql");
            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}
