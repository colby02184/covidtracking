using System.Reflection;
using HealthMonitor.Services.CQRS;

namespace HealthMonitor.Dashboard
{
    public static class WebMiddlewareServices
    {
        public static IServiceCollection SetMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpClient<PublisherGateway>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7260/");

            });
            services.AddScoped<IPublisherGateway, PublisherGateway>();
            services.AddScoped<IBus, RemoteableBus>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            return services;
        }
    }
}
