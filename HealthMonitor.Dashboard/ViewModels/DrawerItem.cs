using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;

namespace HealthMonitor.Dashboard.ViewModels
{
    public class DrawerItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public ISvgIcon Icon { get; set; }
        public bool Separator { get; set; }
        public RenderFragment? CustomIcon { get; set; }
        public static DrawerItem None() => new();
    }
}
