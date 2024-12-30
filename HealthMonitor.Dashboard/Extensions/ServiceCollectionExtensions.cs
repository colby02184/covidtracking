using System.Reflection;
using System.Security.Claims;
using FluentValidation;
using HealthMonitor.Dashboard.Validators;
using HealthMonitor.Framework.Options;
using HealthMonitor.Services.CQRS;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using TokenHandler = HealthMonitor.Dashboard.Utilities.TokenHandler;

namespace HealthMonitor.Dashboard.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            AddThirdPartyServices(services);
           
            AddHttpClientServices(services, configuration);

            AddDependencies(services);

            AddAuthenticationServices(services, configuration);

            AddOptionsServices(services, configuration);

            return services;
        }

        private static void AddThirdPartyServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTelerikBlazor();
            services.AddControllers();

            services.AddRazorComponents().AddInteractiveServerComponents();

        }

        private static void AddOptionsServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        }

        private static void AddHttpClientServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<MessageGatewayPublisher>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiUrl"]);
                client.Timeout = TimeSpan.FromSeconds(500);

            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddHttpMessageHandler<TokenHandler>();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() 
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static void AddAuthenticationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = configuration["Okta:ClientId"];
                    options.ClientSecret = configuration["Okta:ClientSecret"];
                    options.Authority = $"{configuration["Okta:Domain"]}/oauth2/default";
                    options.CallbackPath = "/authentication/login-callback";
                    options.SignedOutCallbackPath = "/authentication/logout-callback";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "roles";
                    //options.TokenValidationParameters.ValidateIssuer = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                   
                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var accessToken = context.TokenEndpointResponse?.AccessToken;
                            if (string.IsNullOrEmpty(accessToken)) return Task.CompletedTask;
                            var identity = context.Principal?.Identity as ClaimsIdentity;
                            identity?.AddClaim(new Claim("access_token", accessToken));

                            var nameClaim = context.Principal?.FindFirst("name");
                            if (nameClaim != null)
                            {
                                identity?.AddClaim(new Claim(ClaimTypes.Name, nameClaim.Value));
                            }

                            return Task.CompletedTask;
                        }
                    };

                });
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CovidCaseViewModelValidator>();
            services.AddScoped<IPublisherGateway, MessageGatewayPublisher>();
            services.AddScoped<IBus, RemoteableBus>();

            services.AddScoped<TokenHandler>();
            services.AddHttpContextAccessor();

        }
    }
}
