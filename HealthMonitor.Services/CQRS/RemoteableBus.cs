using MediatR;

namespace HealthMonitor.Services.CQRS
{
    public class RemoteableBus : Bus
    {
        private PublisherGateway _gateway;

        public RemoteableBus(IMediator mediator, PublisherGateway gateway) : base(mediator)
        {
            _gateway = gateway;
        }

        public override async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            if (request is IRemoteableRequest remotableRequest)
            {
                MessagePayload result = await _gateway.Publish(remotableRequest) ?? throw new InvalidOperationException();
                TResponse returnEvent = result.GetBodyObject<TResponse>();
                return returnEvent;
            } 

            return await base.Send(request);
        }
    }
}
