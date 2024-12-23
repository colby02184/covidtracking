using AutoMapper;
using HealthMonitor.Dashboard.ViewModels;
using HealthMonitor.Framework;
using HealthMonitor.Services.CQRS;
using HealthMonitor.Services.CQRS.Queries;
using Microsoft.AspNetCore.Components;

namespace HealthMonitor.Dashboard.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] public IBus Bus { get; set; }
        [Inject] public IMapper Mapper { get; set; }
        private bool _isDataLoaded = false;

        private CovidSummaryViewModel? CovidSummary;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_isDataLoaded)
            {
                var response = await Bus.Send(new GetCovidSummary.Query());
                CovidSummary = response.IsSuccess ? Mapper.Map<CovidSummaryViewModel>(response.Data) : null;
                _isDataLoaded = true;
                StateHasChanged();
            }
        }
    }

}
