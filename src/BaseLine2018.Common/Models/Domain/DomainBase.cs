using System;


namespace BaseLine2018.Common.Models.Domain
{
    /// <summary>
    /// A base class for domain object, providing a typical set of common fields.  
    /// This mirrors entity objects  
    /// </summary>
    public class DomainBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
