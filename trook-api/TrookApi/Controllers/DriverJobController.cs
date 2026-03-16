using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var jobs = await db.DriverJobs.ToListAsync();
        return Ok(jobs);
    }
}