using Microsoft.Extensions.DependencyInjection;

using BaseLine2018.Service.Interface.Sample1Services;
using BaseLine2018.Service.Sample1Services;


namespace BaseLine2018.Service.StartupExtensions
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddServiceServices(this IServiceCollection services)
        {
            RegisterServiceServices(services);
            return services;
        }

        private static void RegisterServiceServices(IServiceCollection services)
        {
            services.AddScoped<ISampleService, SampleService>();
        }
    }
}
