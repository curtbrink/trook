using Microsoft.AspNetCore.Mvc;
using TrookApi.Database;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/jobs")]
public class DriverJobController(TrookDbContext db, ILogger<DriverJobController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        logger.LogInformation("Getting all driver jobs");
        var jobs = db.DriverJobs.ToList();
        return Ok(jobs);
    }
}