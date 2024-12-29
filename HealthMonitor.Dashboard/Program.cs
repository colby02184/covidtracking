using HealthMonitor.Dashboard.Components;
using HealthMonitor.Dashboard.Extensions;
using HealthMonitor.Dashboard.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.MapControllers();

app.AddApplicationMiddlewares(app.Environment);

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapHub<StatusHub>("/statusHub");

app.Run();
