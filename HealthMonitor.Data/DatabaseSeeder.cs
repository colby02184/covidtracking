using HealthMonitor.Framework;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
                throw new InvalidOperationException("Had an issue getting data from https://api.covidtracking.com");
            }

            var stateLongLatDataDic = await GetLonLatStateDataAsync();
            
            foreach (var data in covidDataList.Where(data => !dbContext.CovidData.Any(cd => cd.Date == data.Date)))
            {
                if (data is { HospitalizedCurrently: not null, TotalTestResults: > 0 })
                {
                    data.HospitalizationRate =
                        Math.Round((double)data.HospitalizedCurrently.Value / data.TotalTestResults.Value * 100, 2);
                }
                else
                {
                    data.HospitalizationRate = null; 
                }

                if (data.State != null)
                {

                    if (stateLongLatDataDic.TryGetValue(data.State.ToUpper(), out var coordinates))
                    {
                        data.Latitude = coordinates.Latitude;
                        data.Longitude = coordinates.Longitude;
                    }
                }

                dbContext.CovidData.Add(data);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task<Dictionary<string, (double Latitude, double Longitude)>> GetLonLatStateDataAsync()
        {
            var stateAbbreviations = new List<string>
            {
                "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS",
                "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY",
                "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV",
                "WI", "WY"
            };

            var stateCoordinates = new Dictionary<string, (double Latitude, double Longitude)>();

            foreach (var state in stateAbbreviations)
            {
                var apiUrl = $"https://geocode.maps.co/search?q={state}&api_key=67698308be1fd765077074ydw6ab957";

                var response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(json);
                var root = jsonDocument.RootElement;

                if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                {
                    root = root[0]; 
                }

                var latitude = double.Parse(root.GetProperty("lat").GetString() ?? "0");
                var longitude = double.Parse(root.GetProperty("lon").GetString() ?? "0");
               
                stateCoordinates[state] = (latitude, longitude);


                await Task.Delay(1000);
            }
            return stateCoordinates;
        }
    }
}
