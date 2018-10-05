using System.Collections.Generic;
using BaseLine2018.Common.Enums;

namespace BaseLine2018.Common.Models.Domain
{
    /// <summary>
    /// Project intended to provide a Generalised container for the response from a service class.
    /// Typical intened use is a container to relay either:
    /// a) valid data (e.g. a dataset returned from a database query)
    /// b) error information (e.g. why a database call failed)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResponse<T>
    {
        public ServiceResponseStatusEnum ServiceResponseStatus { get; set; } // general ok/fail status
        public string Message { get; set; } //  optional supplementary message (e.g. "total records found")
        public T Result { get; set; } // the result (e.g. a list of records), or an error message (when in error state)

        public ServiceResponse()
        {
            ServiceResponseStatus = ServiceResponseStatusEnum.Unset;
            Message = string.Empty;
        }
    }
}
