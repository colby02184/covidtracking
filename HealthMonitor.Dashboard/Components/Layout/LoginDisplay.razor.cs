﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace HealthMonitor.Dashboard.Components.Layout
{
    public partial class LoginDisplay
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public string ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ReturnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        }

        private void HandleLogin(MouseEventArgs obj)
        {
            NavigationManager.NavigateTo($"Login?returnUrl={ReturnUrl}");
        }

        private void HandleLogout(MouseEventArgs obj)
        {
            NavigationManager.NavigateTo($"Logout?returnUrl={ReturnUrl}");
        }
    }
}