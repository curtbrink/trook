using Microsoft.AspNetCore.Mvc;
using TrookApi.Database;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/trook")]
public class TrookController(TrookDbContext db, ILogger<TrookController> logger) : ControllerBase
{
    [HttpGet("hello")]
    public async Task<IActionResult> Get()
    {
        var json = "{\"foo\":\"Hello world!\"}";
        logger.LogInformation("Hello world!");
        return Ok(json);
    }

    [HttpGet("jobs")]
    public async Task<IActionResult> GetJobs()
    {
        logger.LogInformation("Getting all driver jobs");
        var jobs = db.DriverJobs.ToList();
        return Ok(jobs);
    }
}