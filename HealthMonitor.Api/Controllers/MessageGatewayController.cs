using HealthMonitor.Services.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthMonitor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageGatewayController(IBus bus, ILogger<MessageGatewayController> logger) : ControllerBase
    {
        [HttpGet]
        public ActionResult IsAlive() => new OkObjectResult(new Response<bool>(true));

        [HttpPost]
        public async Task<IActionResult> Post(MessagePayload messagePayload)
        {
            try
            {
                var result = await bus.Send(messagePayload.GetBodyObject()) ?? throw new InvalidOperationException();
                var response = new MessagePayload(result).GetJson();

                if (logger.IsEnabled(LogLevel.Trace))
                {

                }

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
