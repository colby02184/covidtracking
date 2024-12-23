using System.Reflection;
using HealthMonitor.Data;
using HealthMonitor.Services.CQRS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMonitor.Services
{
    public static class ServiceMiddlewareServices
    {
        public static IServiceCollection SetServiceMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.SetDataMiddlewareServices(configuration);


            return services;
        }
    }
}
