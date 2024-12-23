using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace HealthMonitor.Services.CQRS
{
    public class PublisherGateway : IPublisherGateway
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PublisherGateway> _logger;

        public PublisherGateway(HttpClient httpClient, ILogger<PublisherGateway> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<MessagePayload> Publish(IRemoteableRequest request)
        {
            var message = new MessagePayload(request);
            return await SendTopic(message);
        }

        public virtual async Task<MessagePayload?> SendTopic(MessagePayload messagePayload)
        {
            var response = await _httpClient.PostAsJsonAsync("api/MessageGateway", messagePayload);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
            {
                return new MessagePayload(new Response<string>(
                    data: null,
                    isSuccess: false,
                    errors: new List<ResponseError>
                    {
                        new()
                        {
                            Message = "Something went wrong communicating with the API.",
                            Code = response.StatusCode.ToString()
                        }
                    }));

            }

            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.Log(LogLevel.Trace,
                    "Sent {MessagePayload}",
                    new { MACHINE = Environment.MachineName, REQUEST_BODY = messagePayload.GetJson(), END_POINT = response.RequestMessage.RequestUri?.AbsoluteUri ?? string.Empty, RESPONSE_BODY = responseContent });
            }

            return JsonSerializer.Deserialize<MessagePayload>(responseContent);
        }
    }
}
