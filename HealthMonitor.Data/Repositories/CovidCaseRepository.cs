using HealthMonitor.Framework;
using Microsoft.EntityFrameworkCore;

namespace HealthMonitor.Data.Repositories
{
    public class CovidCaseRepository(ApplicationDbContext dbContext) : ICovidCaseRepository
    {
        public async Task<List<CovidData>> GetAllAsync() => await dbContext.CovidData.ToListAsync();

        public async Task<CovidData> GetByIdAsync(int id) => await dbContext.CovidData.FindAsync(id);

        public async Task AddAsync(CovidData covidData)
        {
            await dbContext.CovidData.AddAsync(covidData);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CovidData covidData)
        {
            dbContext.CovidData.Update(covidData);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await dbContext.CovidData.FindAsync(id);
            if (data != null)
            {
                dbContext.CovidData.Remove(data);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CovidData>> GetByDateAsync(DateTime date)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Date == date)
                .ToListAsync();
        }

        public async Task<List<CovidData>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Date >= startDate && cd.Date <= endDate)
                .ToListAsync();
        }

        public async Task<List<CovidData>> GetByPositiveCasesAsync(int minCases)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Positive >= minCases)
                .ToListAsync();
        }

        public async Task<List<CovidData>> GetByDeathsAsync(int minDeaths)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Death >= minDeaths)
                .ToListAsync();
        }

        public async Task<int> GetTotalPositiveCasesAsync(DateTime startDate, DateTime endDate)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Date >= startDate && cd.Date <= endDate)
                .SumAsync(cd => cd.Positive ?? 0);
        }

        public async Task<double> GetAverageHospitalizationsAsync(DateTime startDate, DateTime endDate)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Date >= startDate && cd.Date <= endDate)
                .AverageAsync(cd => cd.HospitalizedCurrently ?? 0);
        }

        public async Task<int> GetMaxDeathsAsync(DateTime startDate, DateTime endDate)
        {
            return await dbContext.CovidData
                .Where(cd => cd.Date >= startDate && cd.Date <= endDate)
                .MaxAsync(cd => cd.Death ?? 0);
        }

        public async Task<List<CovidData>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            return await dbContext.CovidData
                .OrderBy(cd => cd.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
