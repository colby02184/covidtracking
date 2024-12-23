using AutoMapper;
using HealthMonitor.Dashboard.ViewModels;
using HealthMonitor.Framework;

namespace HealthMonitor.Dashboard.Profiles
{
    public class CovidProfile: Profile
    {
        public CovidProfile()
        {
            CreateMap<CovidCaseViewModel, CovidData>().ReverseMap();
            CreateMap<CovidSummaryViewModel, CovidSummary>().ReverseMap();
        }
    }
}
