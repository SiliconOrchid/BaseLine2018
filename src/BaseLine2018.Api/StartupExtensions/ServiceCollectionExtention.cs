using Microsoft.Extensions.DependencyInjection;

using AspNetCoreRateLimit;

namespace BaseLine2018.Api.StartupExtensions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            RegisterApiServices(services);
            return services;
        }

        private static void RegisterApiServices(IServiceCollection services)
        {
            //hosting
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); // JM18Jul18 : This samplecode is written for single-instance servers, be aware of how this works in a load-balanced environment - read the docs https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }
    }
}