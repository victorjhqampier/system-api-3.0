using Microsoft.AspNetCore.Mvc;
using SystemAPI.Models.Internals;

namespace SystemAPI.Controllers.ExampleBasic;

[Route("service-domain-s/v1/example-behavior-qualifier")]
[ApiController]
public class ExampleController : ControllerBase
{
    [HttpGet("retrieve")]
    public IActionResult Get(
        [FromHeader(Name = "x-device-identifier")] string? deviceIdentifier,
        [FromHeader(Name = "x-message-identifier")] string? messageIdentifier,
        [FromHeader(Name = "x-channel-identifier")] string? channelIdentifier
    )
    {
        return Ok(new
        {
            Message = "This is an example response.",
            HeaderInfo = channelIdentifier
        });
    }
}
