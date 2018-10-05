using Microsoft.Extensions.DependencyInjection;

using BaseLine2018.Data.Repository;
using BaseLine2018.Data.Repository.Interfaces;


namespace BaseLine2018.Data.StartupExtensions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            RegisterDataServices(services);
            return services;
        }

        private static void RegisterDataServices(IServiceCollection services)
        {
            services.AddScoped<ISampleEntityRepository, SampleEntityRepository>();
        }
    }
}
