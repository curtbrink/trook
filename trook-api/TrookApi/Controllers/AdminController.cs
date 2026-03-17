using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/admin")]
public class AdminController(
    FileService fileService,
    DriverJobService driverJobService,
    TrookDbContext db,
    ILogger<AdminController> logger) : ControllerBase
{
    [HttpPost("clear-all")]
    public async Task<IActionResult> ClearData()
    {
        logger.LogInformation("Clearing all data!");
        await db.DriverJobs.ExecuteDeleteAsync();
        // await db.ProcessedFiles.ExecuteDeleteAsync();
        return Ok();
    }
}