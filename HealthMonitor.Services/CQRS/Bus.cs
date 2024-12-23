using MediatR;

namespace HealthMonitor.Services.CQRS
{
    public class Bus : IBus
    {
        private readonly IMediator _mediator;

        public Bus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public virtual Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            try
            {
                return _mediator.Send(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public virtual Task<object?> Send(object request)
        {
            try
            {
                return _mediator.Send(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}