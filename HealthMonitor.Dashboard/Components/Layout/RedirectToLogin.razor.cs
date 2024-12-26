using Microsoft.AspNetCore.Components;

namespace HealthMonitor.Dashboard.Components.Layout
{
    public partial class RedirectToLogin
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var returnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            NavigationManager.NavigateTo($"login?returnUrl={returnUrl}", true);
        }
    }
}
