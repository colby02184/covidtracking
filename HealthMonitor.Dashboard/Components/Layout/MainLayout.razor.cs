using HealthMonitor.Dashboard.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace HealthMonitor.Dashboard.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager Navigation { get; set; } = null!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        bool DrawerExpanded { get; set; } = true; // drawer expanded by default
        DrawerItem SelectedItem { get; set; } = DrawerItem.None(); // set in OnInitialized
        TelerikDrawer<DrawerItem> DrawerRef { get; set; } = default!; // set by framework
        // in this sample we hardcode the existing pages, in your case you can
        // create the list based on your business logic (e.g., based on user roles/access)
        List<DrawerItem> NavigablePages { get; } =
        [
            new() { Text = "Home", Url = "/", Icon = SvgIcon.Home },
            new() { Separator = true },
            new() { Text = "Covid", Url = "covid", Icon = SvgIcon.WarningCircle}
        ];
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (!user.Identity.IsAuthenticated)
            {
                Navigation.NavigateTo("/login");
                return;
            }

            if (NavigablePages.All(p => p.Text != "Profile"))
            {
                NavigablePages.Add(new() { Separator = true });
                NavigablePages.Add(new DrawerItem { Text = "Profile", Url = "profile", Icon = SvgIcon.User });
            }
        }

        protected override void OnInitialized()
        {
            // pre-select the page the user lands on
            // as the user clicks items, the DOM changes only in the Body and so the selected item stays active
            SelectedItem = NavigablePages.Where(HasUrlEqualToPageRoute).FirstOrDefault(DrawerItem.None());

            base.OnInitialized();
        }

        bool HasUrlEqualToPageRoute(DrawerItem item)
        {
            if (item.Url is null) return false;

            string navItemAsAbsoluteUri = Navigation.ToAbsoluteUri(item.Url).AbsoluteUri;
            if (string.Equals(Navigation.Uri, navItemAsAbsoluteUri, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            // Special case: highlight links to http://host/path/ even if you're
            // at http://host/path (with no trailing slash)
            //
            // This is because the router accepts an absolute URI value of "same
            // as base URI but without trailing slash" as equivalent to "base URI",
            // which in turn is because it's common for servers to return the same page
            // for http://host/vdir as they do for host://host/vdir/ as it's no
            // good to display a blank page in that case.
            bool isNotRoot = item.Url != "/";
            bool hasTrailingSlash = Navigation.Uri[Navigation.Uri.Length - 1] == '/';
            if (isNotRoot && hasTrailingSlash &&
            Navigation.Uri.StartsWith(navItemAsAbsoluteUri, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

    }
}
