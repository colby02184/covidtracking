using System.Reflection;
using HealthMonitor.Services.CQRS;

namespace HealthMonitor.Web
{
    public static class WebMiddlewareServices
    {
        public static IServiceCollection SetMiddlewareServices(this IServiceCollection services)
        {
            services.AddHttpClient<PublisherGateway>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7154/");

            });
            services.AddScoped<IPublisherGateway, PublisherGateway>();
            services.AddScoped<IBus, RemoteableBus>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            return services;
        }
    }
}
