using MediatR;

namespace HealthMonitor.Services.CQRS
{
    public class RemoteableBus(ISender sender, MessageGatewayPublisher gateway) : Bus(sender)
    {
        public override async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            switch (request)
            {
                case IRemoteableRequest remoteableRequest:
                {
                    var result = await gateway.Publish(remoteableRequest) ?? throw new InvalidOperationException();
                    return result.GetBodyObject<TResponse>(); 
                }
                default:
                    return await base.Send(request);
            }
        }
    }
}
