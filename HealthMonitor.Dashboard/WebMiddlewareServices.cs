using System.Reflection;
using HealthMonitor.Services.CQRS;

namespace HealthMonitor.Dashboard
{
    public static class WebMiddlewareServices
    {
        public static IServiceCollection SetMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //var apiBaseUrl = s.Environment.IsDevelopment()
            //    ? "http://localhost:5000" // Local API during development
            //    : "https://your-api-name.azurewebsites.net"; // D
            services.AddHttpClient<PublisherGateway>(client =>
            {
                //client.BaseAddress = new Uri("https://localhost:7260/");
                client.BaseAddress = new Uri("https://healthmonitordashboard20241223070619.azurewebsites.net/");
                client.Timeout = TimeSpan.FromSeconds(500);

            });
            services.AddScoped<IPublisherGateway, PublisherGateway>();
            services.AddScoped<IBus, RemoteableBus>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


            return services;
        }
    }
}
