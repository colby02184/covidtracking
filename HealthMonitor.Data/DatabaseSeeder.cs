using HealthMonitor.Framework;
using System.Text.Json;

namespace HealthMonitor.Data
{
    public class DatabaseSeeder(ApplicationDbContext dbContext, HttpClient httpClient)
    {
        public async Task SeedDatabaseAsync()
        {
            // Fetch data from the API
            var response = await httpClient.GetAsync("https://api.covidtracking.com/v1/states/daily.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var covidDataList = JsonSerializer.Deserialize<List<CovidData>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (covidDataList == null || !covidDataList.Any())
            {
                throw new InvalidOperationException("No data found from the API.");
            }

            // Add data to the database
            foreach (var data in covidDataList.Where(data => !dbContext.CovidData.Any(cd => cd.Date == data.Date)))
            {
                if (data is { HospitalizedCurrently: not null, TotalTestResults: > 0 })
                {
                    data.HospitalizationRate = Math.Round((double)data.HospitalizedCurrently.Value / data.TotalTestResults.Value * 100, 2);
                }
                else
                {
                    data.HospitalizationRate = null; // Set to null if data is incomplete
                }
                dbContext.CovidData.Add(data);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
