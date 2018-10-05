
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace BaseLine2018.Api.StartupExtensions
{
    public static class LoggingStartupExtension
    {
        public static void SetLogging(IApplicationBuilder app, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            // "MappedDiagnosticsLogicalContext" is a way to define a key-value pair using NLog - this value can be passed into the nlog.config file as options
            // In the following case, we want to pass the Azure-Storage connection string from appsettings, into the nlog.config file (and avoid hardcoding the connection in the nlog.config)
            // JM13Aug18 : Please refer to CBWT-25, along with https://github.com/damienbod/AspNetCoreNlog to figure out what is going on.
            // The key takeaway here is to match up the key "azureBlobStorageconnectionString" with the tag "${mdlc:item=azureBlobStorageconnectionString}" in the nlog.config file.
            MappedDiagnosticsLogicalContext.Set("azureBlobStorageconnectionString", configuration.GetConnectionString("AzureStorage"));


            // define loggerFactory configuration:
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddNLog();
            loggerFactory.AddAzureWebAppDiagnostics();
            loggerFactory.ConfigureNLog("nlog.config");
            app.AddNLogWeb();
        }
    }
}
