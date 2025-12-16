using Application.Internals.Adapters;
using Application.Ports;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;
using SystemAPI.Helpers;
using SystemAPI.Models;

namespace SystemAPI.Controllers.ExampleBian;

[Route("service-domain-s/v1/example2-behavior-qualifier")]
[ApiController]
public class BianExampleController : ControllerBase
{
    private readonly IExamplePort _exampleUsecase;
    private readonly ILogger<BianExampleController> _logger;

    public BianExampleController(ILogger<BianExampleController> logger, IExamplePort exampleUsecase)
    {   
        _logger = logger;
        _exampleUsecase = exampleUsecase;
    }

    [HttpGet("retrieve")]
    public async Task<IActionResult> Register(
        [FromHeader(Name = "x-device-identifier")] string? deviceIdentifier,
        [FromHeader(Name = "x-message-identifier")] string? messageIdentifier,
        [FromHeader(Name = "x-channel-identifier")] string? channelIdentifier
    ){
        var headers = new TraceIdentifierAdapter
        {
            ChannelIdentifier = channelIdentifier,
            DeviceIdentifier = deviceIdentifier,
            MessageIdentifier = messageIdentifier
        };

        try
        {
            var (success, fail) = await _exampleUsecase.ShowExampleAsync(headers);

            if (fail is not null)
            {
                _logger.LogWarning("TraceId=[{Headers}] Validation=[{ValidationErrors}]", LoggerMapperHelper.ToString(headers), LoggerMapperHelper.ToString(fail.Validations.FirstOrDefault()!));
                return StatusCode(fail.StatusGroup, EasyResponseBianHelper.EasyWarningRespond<BianExampleResponseModel>(fail.Validations));
            }

            if (success is null)
            {
                _logger.LogWarning("TraceId=[{Headers}]", LoggerMapperHelper.ToString(headers));
                return NoContent();
            }

            return Ok(EasyResponseBianHelper.EasySuccessRespond<BianExampleResponseModel>(success));
        }
        catch (Exception ex)
        {
            var tracer = Regex.Replace(ex.StackTrace ?? "", @"\sat\s(.*?)\sin\s", string.Empty).Trim();
            _logger.LogError("Message=[{Message}] TraceId=[{Headers}] StackTrace={Trace}", ex.Message, LoggerMapperHelper.ToString(headers), tracer);

            return StatusCode(500, EasyResponseBianHelper.EasyErrorRespond());
        }
    }
}
