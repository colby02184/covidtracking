using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMonitor.Framework;

namespace HealthMonitor.Data.Repositories
{
    public class CovidSummaryService(HttpClient httpClient) : ICovidSummaryService
    {
        public async Task<CovidSummary?> GetCovidSummaryAsync()
        {
            var response = await httpClient.GetAsync("https://api.covidtracking.com/v1/us/current.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CovidSummary>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data?.FirstOrDefault();
        }
    }
}
