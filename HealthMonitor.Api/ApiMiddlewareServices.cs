using HealthMonitor.Data;
using HealthMonitor.Services;
using HealthMonitor.Services.CQRS;

namespace HealthMonitor.Api
{
    public static class ApiMiddlewareServices
    {
        public static IServiceCollection SetApiMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.SetDataMiddlewareServices(configuration);
            services.SetServiceMiddlewareServices(configuration);

            services.AddScoped<IBus, Bus>();

            return services;
        }
    }
}
