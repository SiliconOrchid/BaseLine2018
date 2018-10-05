using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BaseLine2018.Common.Extensions;
using BaseLine2018.Common.Logging;
using BaseLine2018.Common.Models.Configuration;
using Microsoft.EntityFrameworkCore;

using BaseLine2018.Common.Models.Entity;
using BaseLine2018.Data.Context;
using BaseLine2018.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BaseLine2018.Data.Repository
{
    /// <summary>
    /// This is a sample data access repository, demonstrating how one might interact with a SampleEntity database item.
    /// It can be safely removed from a production system.
    /// Pay particular attention to the use of a GenericRepository base-class, which provides a set of
    /// general-purpose boiler-plate methods.    
    /// This class can then be used to provide for more specific-purpose methods.
    /// </summary>
    public class SampleEntityRepository : GenericRepository<SampleEntity>, ISampleEntityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly FeatureSwitchesConfig _featureSwitchesConfig;
        private readonly MemoryCachingConfig _memoryCachingConfig;

        public SampleEntityRepository(
            ApplicationDbContext applicationDbContext,
            IMemoryCache memoryCache,
            IOptions<FeatureSwitchesConfig> featureSwitchesConfig,
            IOptions<MemoryCachingConfig> memoryCachingConfig
            ) 
            : base(applicationDbContext, memoryCache, featureSwitchesConfig, memoryCachingConfig)
        {
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException($"{ClassNameExtension.GetCallingClassAndMethod(this)} : applicationContext was null");
            _memoryCache = memoryCache ?? throw new ArgumentNullException($"{ClassNameExtension.GetCallingClassAndMethod(this)} : memoryCache was null"); ;
            _featureSwitchesConfig = featureSwitchesConfig.Value ?? throw new ArgumentNullException($"{ClassNameExtension.GetCallingClassAndMethod(this)} : featureSwitchesConfig was null"); ;
            _memoryCachingConfig = memoryCachingConfig.Value ?? throw new ArgumentNullException($"{ClassNameExtension.GetCallingClassAndMethod(this)} : memoryCachingConfig was null"); ;
        }


        /// <summary>
        /// An example custom repository method
        /// This calls the baseclasses "GetAll()" and then layers on extra LINQ as required
        /// Also demonstrates caching specific to this repo method.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SampleEntity>> GetAllOrderedAsync()
        {
            // default to returning an empty collection rather than null
            IEnumerable<SampleEntity> items = new List<SampleEntity>();

            try
            {
                string cacheKey = $"SampleEntityRepository_GetAllOrderedAsync";

                if (_featureSwitchesConfig.EnableRepositoryMemoryCache && _memoryCache.TryGetValue(cacheKey, out items))
                {
                    // returned cached version
                    return items;
                }

                // no cached version, so query DB and set to cache
                items = await base.GetAll().OrderBy(x => x.SampleTextField).ToListAsync();

                _memoryCache.Set<IEnumerable<SampleEntity>>(cacheKey, items, DateTime.Now.AddSeconds(_memoryCachingConfig.ShortCacheDurationSeconds));

                if (!items.Any())
                {
                    Log.Warn($"{this.GetCallingClassAndMethod()} No SampleEntity records found");
                }

                return items;
            }
            catch (Exception ex)
            {
                Log.Error($"{this.GetCallingClassAndMethod()} Unexpected exception : ", ex);
                throw;
            }

        }


    }
}
