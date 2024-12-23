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
                dbContext.CovidData.Add(data);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
