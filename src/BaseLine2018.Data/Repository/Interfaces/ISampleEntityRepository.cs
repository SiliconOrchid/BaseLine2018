using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLine2018.Common.Models.Domain;
using BaseLine2018.Common.Models.Entity;

namespace BaseLine2018.Data.Repository.Interfaces
{
    public interface ISampleEntityRepository : IGenericRepository<SampleEntity>
    {
        Task<IEnumerable<SampleEntity>> GetAllOrderedAsync();
    }
}
