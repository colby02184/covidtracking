using HealthMonitor.Dashboard.Components;

namespace HealthMonitor.Dashboard.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddApplicationMiddlewares(this IApplicationBuilder app, IHostEnvironment? env = null)
        {
            env ??= app.ApplicationServices.GetRequiredService<IHostEnvironment>();

            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAntiforgery();
            return app;
        }
    }
}
