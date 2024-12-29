using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor;

namespace HealthMonitor.Dashboard.Components.Layout
{
    public partial class LoginDisplay
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public string ReturnUrl { get; set; }

        private string LogoutButtonClass => $"{ThemeConstants.Button.FillMode.Link} link-button";
        private string LoginButtonClass => $"{ThemeConstants.Button.FillMode.Link} link-button";

        protected override async Task OnInitializedAsync()
        {
            ReturnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }

        private void HandleLogin(MouseEventArgs obj)
        {
            NavigationManager.NavigateTo($"Login?returnUrl={ReturnUrl}", forceLoad: true);
        }

        private void HandleLogout(MouseEventArgs obj)
        {
            NavigationManager.NavigateTo($"Logout?returnUrl={ReturnUrl}", forceLoad: true);
        }

        private void HandleProfile(MouseEventArgs arg)
        {
            NavigationManager.NavigateTo("/profile");
        }
    }
}
