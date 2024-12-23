using System.Reflection;
using HealthMonitor.Services.CQRS;
using HealthMonitor.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMonitor.Services
{
    public static class ServiceMiddlewareServices
    {
        public static IServiceCollection SetServiceMiddlewareServices(this IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IBus, Bus>();
            

            return services;
        }
    }
}
