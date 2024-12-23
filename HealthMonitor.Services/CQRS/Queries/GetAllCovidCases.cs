using HealthMonitor.Data.Repositories;
using HealthMonitor.Framework;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMonitor.Services.CQRS.Queries
{
    
    public class GetAllCovidCases
    {
        public record Query() : IRequest<Response<List<CovidData>?>>, IRemoteableRequest;
        public class Handler : QueryHandler<Query, List<CovidData>?>
        {
            private readonly ICovidCaseRepository _repository;

            public Handler(ILogger<Handler> logger, ICovidCaseRepository repository) : base(logger)
            {
                _repository = repository;
                QueryFunc = QueryHandler;
            }

            private async Task<List<CovidData>?> QueryHandler(Query request)
            {
                var data = await _repository.GetAllAsync();
                return data;
            }
        }
    }
}
