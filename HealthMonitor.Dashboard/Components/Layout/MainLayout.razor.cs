using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace HealthMonitor.Dashboard.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (!user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/login");
            }
        }
    }
}
