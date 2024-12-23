using AutoMapper;
using HealthMonitor.Dashboard.ViewModels;
using HealthMonitor.Framework;
using HealthMonitor.Services.CQRS;
using HealthMonitor.Services.CQRS.Queries;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace HealthMonitor.Dashboard.Components.Pages
{
    public partial class COVID : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] public IBus Bus { get; set; }
        [Inject] public IMapper Mapper { get; set; }
        public List<CovidCaseViewModel> Data { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();

            await base.OnInitializedAsync();
        }

        private async Task LoadData()
        {
            var response = await Bus.Send(new GetAllCovidCases.Query());
            Data = response.IsSuccess ? Mapper.Map <List<CovidData> ,List <CovidCaseViewModel>>(response.Data) : new();
        }

        async Task Update(GridCommandEventArgs args)
        {
            var index = Data.FindIndex(item => item.Id.Equals(((CovidCaseViewModel)args.Item).Id));

            Data[index] = (CovidCaseViewModel)args.Item;
        }

        async Task Add(GridCommandEventArgs args)
        {
            ((CovidCaseViewModel)args.Item).Id = Data.Any() ? Data.Max(item => item.Id) + 1 : 1;

            Data.Add((CovidCaseViewModel)args.Item);
        }

        async Task Delete(GridCommandEventArgs args)
        {
            Data.RemoveAll(item => item.Id.Equals(((CovidCaseViewModel)args.Item).Id));
        }
    }
}
