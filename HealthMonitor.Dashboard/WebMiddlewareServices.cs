using System.Reflection;
using FluentValidation;
using HealthMonitor.Dashboard.Validators;
using HealthMonitor.Services.CQRS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace HealthMonitor.Dashboard
{
    public static class WebMiddlewareServices
    {
        public static IServiceCollection SetMiddlewareServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
           
            services.AddHttpClient<PublisherGateway>(client =>
            {
                client.BaseAddress = new Uri(configuration["ApiUrl"]);
                client.Timeout = TimeSpan.FromSeconds(500);

            });
            services.AddValidatorsFromAssemblyContaining<CovidCaseViewModelValidator>();
            services.AddScoped<IPublisherGateway, PublisherGateway>();
            services.AddScoped<IBus, RemoteableBus>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            //services.AddAuthentication(authOptions =>
            //{
            //    authOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    authOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    authOptions.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    authOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //}).AddOpenIdConnect(oidcOptions =>
            //{
            //    oidcOptions.Authority = configuration["Okta:Domain"];
            //    oidcOptions.ClientId = configuration["Okta:ClientId"];
            //    oidcOptions.ClientSecret = configuration["Okta:ClientSecret"];
            //    oidcOptions.CallbackPath = "/authorization-code/callback";
            //    oidcOptions.ResponseType = "code";
            //    oidcOptions.SaveTokens = true;
            //    oidcOptions.Scope.Add("openid");
            //    oidcOptions.Scope.Add("profile");
            //    oidcOptions.TokenValidationParameters.ValidateIssuer = false;
            //    oidcOptions.TokenValidationParameters.NameClaimType = "name";
            //}).AddCookie();
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

                    // Redirect URI
                    options.CallbackPath = "/authentication/login-callback";

                    // Validate tokens
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "roles";
                    options.TokenValidationParameters.ValidateIssuer = false;
                });

            return services;
        }
    }
}
