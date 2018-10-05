using System;

namespace BaseLine2018.Common.Exceptions
{
    public class AppSettingsException : ArgumentException
    {
        public AppSettingsException(string message) : base(message) { }
        public AppSettingsException(string message, string paramName) : base(message, paramName) { }
        public AppSettingsException(string message, Exception innerException) : base(message, innerException) { }
        public AppSettingsException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) { }
    }
}
