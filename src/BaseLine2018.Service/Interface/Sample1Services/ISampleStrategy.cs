using System.Collections.Generic;
using System.Threading.Tasks;

using BaseLine2018.Common.Models.Domain;

namespace BaseLine2018.Service.Interface.Sample1Services
{
    public interface ISampleService
    {
        Task<ServiceResponse<Sample>> GetAsync(int id);
        Task<ServiceResponse<List<Sample>>> GetAllAsync();
    }
}
