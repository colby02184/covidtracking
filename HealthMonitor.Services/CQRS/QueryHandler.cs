using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthMonitor.Services.CQRS
{
    public abstract class QueryHandler<TQuery, TResponse>(ILogger<QueryHandler<TQuery, TResponse>> logger) : IRequestHandler<TQuery, Response<TResponse>> 
        where TQuery : IRequest<Response<TResponse>>, IRemoteableRequest
    {
        public Func<TQuery, Task<TResponse?>>? QueryFunc { get; set; }

        public async Task<Response<TResponse>> Handle(TQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return new(await QueryFunc(request) ?? throw new InvalidOperationException());
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private Response<TResponse> HandleException(Exception ex)
        {
            logger.LogError($"Query {typeof(TQuery)} encountered an issue: {ex.Message}", ex);
            return new(default, false, new List<ResponseError>
            {
                new()
                {
                    Message = ex.Message
                }
            });
        }
    }
}