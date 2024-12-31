using MediatR;

namespace HealthMonitor.Services.CQRS
{
    public class Bus(ISender sender) : IBus
    {
        public virtual Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            try
            {
                return sender.Send(request);
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
                return sender.Send(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}