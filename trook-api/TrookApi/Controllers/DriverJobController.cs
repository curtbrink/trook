using Microsoft.AspNetCore.Mvc;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/jobs")]
public class DriverJobController(DriverJobService driverJobService, FileService fileService, ILogger<DriverJobController> logger, PlayerService playerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJobs([FromRoute] Guid profileId)
    {
        logger.LogInformation("Getting all driver jobs");
        var jobs = await driverJobService.GetAllJobsForProfile(profileId);
        return Ok(jobs);
    }
}