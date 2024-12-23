using HealthMonitor.Framework;

namespace HealthMonitor.Data.Repositories
{
    public interface ICovidCaseRepository
    {
        Task<List<CovidData>> GetAllAsync();
        Task<List<CovidData>> GetByDateAsync(DateTime date);
        Task<List<CovidData>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<List<CovidData>> GetByPositiveCasesAsync(int minCases);
        Task<List<CovidData>> GetByDeathsAsync(int minDeaths);

        Task<List<CovidData>> GetPaginatedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalPositiveCasesAsync(DateTime startDate, DateTime endDate);

        Task<double> GetAverageHospitalizationsAsync(DateTime startDate, DateTime endDate);

        Task<int> GetMaxDeathsAsync(DateTime startDate, DateTime endDate);


        Task AddAsync(CovidData covidData);
        Task UpdateAsync(CovidData covidData);
        Task DeleteAsync(int id);

    }
}
