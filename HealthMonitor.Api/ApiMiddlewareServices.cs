using HealthMonitor.Data;
using HealthMonitor.Services;

namespace HealthMonitor.Api
{
    public static class ApiMiddlewareServices
    {
        public static IServiceCollection SetApiMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.SetDataMiddlewareServices(configuration);
            services.SetServiceMiddlewareServices();

            return services;
        }
    }
}
