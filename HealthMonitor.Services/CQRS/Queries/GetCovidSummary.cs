using HealthMonitor.Data.Repositories;
using HealthMonitor.Framework;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMonitor.Services.CQRS.Queries
{
    public class GetCovidSummary
    {
        public record Query() : IRequest<Response<CovidSummary?>>, IRemoteableRequest;
        public class Handler : QueryHandler<Query, CovidSummary?>
        {
            private readonly ICovidSummaryService _repository;

            public Handler(ILogger<Handler> logger, ICovidSummaryService repository) : base(logger)
            {
                _repository = repository;
                QueryFunc = QueryHandler;
            }

            private async Task<CovidSummary?> QueryHandler(Query request)
            {
                var data = await _repository.GetCovidSummaryAsync();
                return data;
            }
        }
    }
}
