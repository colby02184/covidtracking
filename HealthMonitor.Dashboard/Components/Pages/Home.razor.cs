using AutoMapper;
using HealthMonitor.Dashboard.ViewModels;
using HealthMonitor.Services.CQRS;
using HealthMonitor.Services.CQRS.Queries;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace HealthMonitor.Dashboard.Components.Pages
{
    public partial class Home : ComponentBase
    {
        private TelerikNotification NotificationRef { get; set; } = null!;
        [Inject] public IBus Bus { get; set; } = null!;
        [Inject] public IMapper Mapper { get; set; } = null!;
        private bool _isDataLoaded;

        private CovidSummaryViewModel? _covidSummary = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_isDataLoaded)
            {
                var result = await Bus.Send(new GetCovidSummary.Query());
                if (!result.IsSuccess)
                {
                    NotificationRef.Show(new NotificationModel
                    {
                        Text = "Something went wrong retrieving sym!",
                        ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                    });
                    return;
                }

                _covidSummary = result.IsSuccess ? Mapper.Map<CovidSummaryViewModel>(result.Data) : null;
                _isDataLoaded = true;
                StateHasChanged();
            }
        }
    }

}
