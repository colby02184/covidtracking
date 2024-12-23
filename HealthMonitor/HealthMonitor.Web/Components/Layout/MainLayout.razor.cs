using HealthMonitor.Web.ViewModels;
using Telerik.Blazor;
using Telerik.SvgIcons;

namespace HealthMonitor.Web.Components.Layout
{
    public partial class MainLayout
    {

        List<DrawerItem> NavigablePages { get; set; } =
        [
            new() { Text = "Home", Url = "/", Icon = SvgIcon.Home },
            new() { Separator = true },
            new() { Text = "Dashboard", Url = "dashboard", Icon = SvgIcon.GridLayout }
        ];

        private IconType IconType = IconType.Svg;

        protected override async Task OnInitializedAsync()
        {
            IconType = IconType.Font;
        }
     
    }
}
