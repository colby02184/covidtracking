using System.Net.Http.Json;
using System.Text.Json;
using HealthMonitor.Framework.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HealthMonitor.Services.CQRS
{
    public class MessageGatewayPublisher(HttpClient httpClient, ILogger<MessageGatewayPublisher> logger, IOptionsSnapshot<ApiSettings> apiSettingsOptionsSnapshot) : IPublisherGateway
    {
        public async Task<MessagePayload?> Publish(IRemoteableRequest request, CancellationToken cancellationToken = default)
        {
            var message = new MessagePayload(request);
            return await SendTopic(message, cancellationToken);
        }

        public virtual async Task<MessagePayload?> SendTopic(MessagePayload messagePayload, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(apiSettingsOptionsSnapshot.Value.MessageGatewayEndpoint, messagePayload, cancellationToken);

                return await ProcessResponseAsync(response, messagePayload);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while communicating with the API endpoint {Endpoint}.", apiSettingsOptionsSnapshot.Value.MessageGatewayEndpoint);
                return CreateErrorPayload("An exception occurred while communicating with the API.", "EXCEPTION");
            }
        }

        private async Task<MessagePayload?> ProcessResponseAsync(HttpResponseMessage? response, MessagePayload messagePayload)
        {
            if (response == null)
            {
                logger.LogError("The response from the API endpoint {Endpoint} was null.", apiSettingsOptionsSnapshot.Value.MessageGatewayEndpoint);
                return CreateErrorPayload("The API response was null.", "NULL_RESPONSE");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
            {
                logger.LogError("Failed to send message. Status: {Status}, Response: {ResponseContent}",
                    response.StatusCode, responseContent);

                return CreateErrorPayload("Failed to communicate with the API.", response.StatusCode.ToString());
            }

            // log successful response
            LogResponse(response, messagePayload, responseContent);
            
            return DeserializeResponse<MessagePayload>(responseContent);
        }

        private void LogResponse(HttpResponseMessage response, MessagePayload messagePayload, string responseContent)
        {
            logger.LogTrace("Sent message successfully: {@Details}", new
            {
                Machine = Environment.MachineName,
                RequestBody = messagePayload.GetJson(),
                Endpoint = response.RequestMessage?.RequestUri?.AbsoluteUri ?? string.Empty,
                ResponseBody = responseContent,
                response.StatusCode
            });
        }

        private MessagePayload CreateErrorPayload(string message, string errorCode)
        {
            return new MessagePayload(new Response<string?>(
                data: null,
                isSuccess: false,
                errors: [
                    new()
                    {
                        Message = message,
                        Code = errorCode
                    }
                ]));
        }

        private T? DeserializeResponse<T>(string responseContent)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(responseContent);
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Failed to deserialize response content: {ResponseContent}", responseContent);
                return default;
            }
        }
    }
}
