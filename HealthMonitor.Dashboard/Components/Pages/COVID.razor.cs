using AutoMapper;
using HealthMonitor.Dashboard.ViewModels;
using HealthMonitor.Framework;
using HealthMonitor.Services.CQRS;
using HealthMonitor.Services.CQRS.Commands;
using HealthMonitor.Services.CQRS.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor;
using Telerik.Blazor.Components;


namespace HealthMonitor.Dashboard.Components.Pages
{
    public partial class COVID : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] public IBus Bus { get; set; }
        [Inject] public IMapper Mapper { get; set; }
        public List<CovidCaseViewModel> Data { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        private TelerikNotification NotificationRef { get; set; }


        private bool isRendered;

        private bool _isDataLoaded;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_isDataLoaded)
            {
                await LoadData();
                _isDataLoaded = true;
                StateHasChanged();
            }

            if (firstRender)
            {

                var heatmapVms = Data.Where(c => c is
                {
                    Longitude: not null,
                    Latitude: not null,
                    Positive: not null and not 0,
                    TotalTestResults: not 0
                })
                    .Select(c => new HeatMapViewModel
                    {
                        Longitude = c.Longitude,
                        Latitude = c.Latitude,
                        Intensity = Convert.ToDouble(c.HospitalizedCurrently)//(double)(c.Positive / c.TotalTestResults) * 100
                    }).ToList();
                var heatmapData = heatmapVms
                    //.Where(data => data is { Latitude: not null, Longitude: not null, Intensity: not 0 }) 
                    .Select(data => new object[]
                    {
                            data.Latitude,
                            data.Longitude,
                            data.Intensity
                    })
                    .ToList();
                await JSRuntime.InvokeVoidAsync("initializeHeatmap", heatmapData);
                isRendered = true;

            }
        }

        private async Task LoadData()
        {
            var response = await Bus.Send(new GetAllCovidCases.Query());
            Data = response.IsSuccess ? Mapper.Map<List<CovidData>, List<CovidCaseViewModel>>(response.Data) : new();
            Data = Data.OrderByDescending(p => p.Positive).ToList();
        }

        async Task Update(GridCommandEventArgs args)
        {
            //var index = Data.FindIndex(item => item.Id.Equals(((CovidCaseViewModel)args.Item).Id));
            var covidCase = Mapper.Map<CovidCaseViewModel, CovidData>((CovidCaseViewModel)args.Item);
            if (covidCase == null)
            {
                return;
            }
            var response = await Bus.Send(new UpdateCovidCase.Command(new CommandParameters<CovidData>()
            {
                Data = covidCase,
                PageOrComponent = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Segments.LastOrDefault(),
                UserName = "SOmeUser"
            }));

            if (response.IsSuccess)
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Update Successful!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Success
                });
            }
            else
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Something went wrong!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                });
            }
            await LoadData();
            StateHasChanged();
        }

        async Task Add(GridCommandEventArgs args)
        {
            ((CovidCaseViewModel)args.Item).Id = Data.Any() ? Data.Max(item => item.Id) + 1 : 1;
            var response = await Bus.Send(new AddCovidCase.Command(new CommandParameters<CovidData>()
            {
                Data = Mapper.Map<CovidCaseViewModel, CovidData>((CovidCaseViewModel)args.Item),
                PageOrComponent = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Segments.LastOrDefault(),
                UserName = "SomeUser"
            }));


            if (response.IsSuccess)
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "New Covid Case Added Successfully!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Success
                });
            }
            else
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Something went wrong!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                });
            }
            await LoadData();
            StateHasChanged();
        }

        async Task Delete(GridCommandEventArgs args)
        {
            Data.RemoveAll(item => item.Id.Equals(((CovidCaseViewModel)args.Item).Id));

            var response = await Bus.Send(new DeleteCovidCase.Command(new CommandParameters<CovidData>()
            {
                Data = Mapper.Map<CovidCaseViewModel, CovidData>((CovidCaseViewModel)args.Item),
                PageOrComponent = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Segments.LastOrDefault(),
                UserName = "SomeUser"
            }));

            if (response.IsSuccess)
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Delete Successful!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Success
                });
            }
            else
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Something went wrong!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                });
            }
            await LoadData();
            StateHasChanged();
        }
    }
}
