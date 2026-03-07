using Microsoft.AspNetCore.Mvc;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/trook")]
public class TrookController : ControllerBase
{
    [HttpGet("hello")]
    public async Task<IActionResult> Get(ILogger<TrookController> logger)
    {
        var json = "{\"foo\":\"Hello world!\"}";
        logger.LogInformation("Hello world!");
        return Ok(json);
    }
}