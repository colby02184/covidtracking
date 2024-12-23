using AutoMapper;
using HealthMonitor.Framework;
using HealthMonitor.Web.ViewModels;
using Telerik.SvgIcons;

namespace HealthMonitor.Web.Profiles
{
    public class CovidCaseProfile: Profile
    {
        public CovidCaseProfile()
        {
            CreateMap<CovidCaseViewModel, CovidData>().ReverseMap();
        }
    }
}
