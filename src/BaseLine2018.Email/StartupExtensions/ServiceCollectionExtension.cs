using Microsoft.Extensions.DependencyInjection;

using BaseLine2018.Email.EmailSenderStrategy;
using BaseLine2018.Email.Interface;



namespace BaseLine2018.Email.StartupExtensions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            RegisterEmailServices(services);
            return services;
        }

        private static void RegisterEmailServices(IServiceCollection services)
        {
            services.AddScoped<IEmailSenderStrategy, SendGridStrategy>();
        }


    }
}
