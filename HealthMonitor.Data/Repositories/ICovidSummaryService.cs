using HealthMonitor.Framework;

namespace HealthMonitor.Data.Repositories;

public interface ICovidSummaryService
{
    Task<CovidSummary?> GetCovidSummaryAsync();
}