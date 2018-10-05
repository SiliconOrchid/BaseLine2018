﻿using System;
using System.IO;
using System.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using ILogger = Microsoft.Extensions.Logging.ILogger;


using NLog;

namespace BaseLine2018.Common.Logging
{
    /*
    //Example of usage:
    //Take advantage of the "ClassNameHelper" to assist locating fault in code, when reading logs

            try
            {
                throw new Exception("Extinction Event Error");
            }
            catch (Exception ex)
            {
                string errorMessage = "Looks like the moon is crashing into us";
                Log.Error($"[{ClassNameHelper.GetCallingClassAndMethod(this)}] {errorMessage}", ex);
            }

    */

    public static class Log
    {

        private static readonly Logger _fileLogger;
        private static readonly ILogger _logger;

        static Log()
        {
            _fileLogger = LogManager.GetCurrentClassLogger();

            var factory = new LoggerFactory()
                .WithFilter(new FilterLoggerSettings
                {
                    { "Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning },
                    { "System", Microsoft.Extensions.Logging.LogLevel.Warning },
                    { "BaseLine2018.Api", Microsoft.Extensions.Logging.LogLevel.Debug },
                    { "BaseLine2018.Web", Microsoft.Extensions.Logging.LogLevel.Debug },
                });
            _logger = factory.CreateLogger("BaseLine2018");

            var loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // note, 'setbasepath' requires 'microsoft.extensions.configuration.json' nuget
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            factory.AddConsole(loggingConfiguration);
        }


        public static void Info(string message)
        {
            if (IsEnabled(Level.Information))
            {
                Trace.TraceInformation(GetTraceMsg(message));
                LogToFile(message, Level.Information);
                LogToConsole(message, Level.Information);
            }
        }

        public static void Debug(string message)
        {
            if (IsEnabled(Level.Debug))
            {
                Trace.WriteLine(GetTraceMsg(message), Enum.GetName(typeof(Level), Level.Debug));
                LogToFile(message, Level.Debug);
                LogToConsole(message, Level.Debug);
            }
        }

        public static void Warn(string message)
        {
            if (IsEnabled(Level.Warning))
            {
                Trace.TraceWarning(GetTraceMsg(message));
                LogToFile(message, Level.Warning);
                LogToConsole(message, Level.Warning);
            }
        }

        public static void Error(string message, Exception ex = null)
        {
            if (IsEnabled(Level.Error))
            {
                Trace.TraceError(GetTraceMsg(message));
                LogToFile(message, Level.Error, ex);
                LogToConsole(message, Level.Error, ex);
            }
        }

        public static void Verbose(string message)
        {
            if (IsEnabled(Level.Verbose))
            {
                Trace.WriteLine(GetTraceMsg(message), Enum.GetName(typeof(Level), Level.Verbose));
                LogToFile(message, Level.Verbose);
                LogToConsole(message, Level.Verbose);
            }
        }


        private static bool IsEnabled(Level logLevel)
        {
            return true; //Get this from logging.json file (enable/disable various logging outputs)
        }



        private static void LogToFile(string message, Level logLevel, Exception ex = null)
        {
            NLog.LogLevel nlogLevel;
            switch (logLevel)
            {
                case Level.Error:
                    nlogLevel = NLog.LogLevel.Error;
                    break;
                case Level.Debug:
                    nlogLevel = NLog.LogLevel.Debug;
                    break;
                case Level.Warning:
                    nlogLevel = NLog.LogLevel.Warn;
                    break;
                case Level.Information:
                    nlogLevel = NLog.LogLevel.Info;
                    break;
                case Level.Verbose:
                    nlogLevel = NLog.LogLevel.Trace;
                    break;
                default:
                    nlogLevel = NLog.LogLevel.Debug;
                    break;
            }

            _fileLogger.Log(nlogLevel, ex, message);
        }

        private static void LogToConsole(string message, Level logLevel, Exception ex = null)
        {
            switch (logLevel)
            {
                case Level.Error:
                    _logger.LogError(new EventId(1), ex, message);
                    break;
                case Level.Debug:
                    _logger.LogDebug(new EventId(1), ex, message);
                    break;
                case Level.Warning:
                    _logger.LogWarning(new EventId(1), ex, message);
                    break;
                case Level.Information:
                    _logger.LogInformation(new EventId(1), ex, message);
                    break;
                case Level.Verbose:
                    _logger.LogTrace(new EventId(1), ex, message);
                    break;
                default:
                    _logger.LogDebug(new EventId(1), ex, message);
                    break;
            }
        }

        private static string GetTraceMsg(string message)
        {
            return $"{DateTime.Now.ToString("u")} {message}";
        }

        public enum Level
        {
            Verbose,
            Warning,
            Debug,
            Error,
            Information
        }
    }
}

