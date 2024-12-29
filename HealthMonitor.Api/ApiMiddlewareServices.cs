using HealthMonitor.Data;
using HealthMonitor.Services;
using HealthMonitor.Services.CQRS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HealthMonitor.Api
{
    public static class ApiMiddlewareServices
    {
        public static IServiceCollection SetApiMiddlewareServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.SetDataMiddlewareServices(configuration);
            services.SetServiceMiddlewareServices(configuration);

            services.AddScoped<IBus, Bus>();

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Okta:Issuer"];//"https://{yourOktaDomain}/oauth2/default"; // Replace with your Okta domain
                    options.Audience = configuration["Okta:Audience"];//"api://default"; // Match the audience in Okta's authorization server
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
