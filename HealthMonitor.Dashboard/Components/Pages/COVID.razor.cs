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
using HealthMonitor.Dashboard.Validators;


namespace HealthMonitor.Dashboard.Components.Pages
{
    public partial class Covid : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] public IBus Bus { get; set; } = null!;
        [Inject] public IMapper Mapper { get; set; } = null!;
        public List<CovidCaseViewModel> Data { get; set; } = new();
        [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
        private TelerikNotification NotificationRef { get; set; } = null!;
        private readonly CovidCaseViewModelValidator _validator = new();


        private bool _isRendered;

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
                await JsRuntime.InvokeVoidAsync("initializeHeatmap", heatmapData);
                _isRendered = true;

            }
        }

        private async Task LoadData()
        {
            var result = await Bus.Send(new GetAllCovidCases.Query());
            if (!result.IsSuccess)
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Something went wrong retrieving Covid cases!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                });
                return;
            }

            Data = Mapper.Map<List<CovidData>, List<CovidCaseViewModel>>(result.Data ?? new ());
            Data = Data.OrderByDescending(p => p.Positive).ToList();
        }

        private async Task Update(GridCommandEventArgs args)
        {
            //var index = Data.FindIndex(item => item.Id.Equals(((CovidCaseViewModel)args.Item).Id));
            var covidCase = Mapper.Map<CovidCaseViewModel, CovidData>((CovidCaseViewModel)args.Item);
            if (covidCase == null)
            {
                return;
            }
            var validator = new CovidCaseViewModelValidator();
            var validationResult = await validator.ValidateAsync((CovidCaseViewModel)args.Item);
            if (!validationResult.IsValid)
            {
                NotificationRef.Show(new NotificationModel
                {
                    Text = "Validation Failed!",
                    ThemeColor = ThemeConstants.AppBar.ThemeColor.Error
                });
                args.IsCancelled = true;
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
