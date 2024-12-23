using System.Reflection;
using HealthMonitor.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMonitor.Data
{
    public static class DataMiddlewareServices
    {
        public static IServiceCollection SetDataMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDbContext<DemoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICovidCaseRepository, CovidCaseRepository>();

            return services;
        }

    }
}
