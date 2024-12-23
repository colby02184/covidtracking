using Telerik.SvgIcons;

namespace HealthMonitor.Web.ViewModels
{
    public class DrawerItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public ISvgIcon Icon { get; set; }
        public bool Separator { get; set; }
    }
}
