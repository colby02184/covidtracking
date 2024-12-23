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
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("CovidInMemoryDb"));
            services.AddScoped<ICovidCaseRepository, CovidCaseRepository>();
            services.AddScoped<ICovidSummaryService, CovidSummaryService>();
            
            return services;
        }

    }
}
