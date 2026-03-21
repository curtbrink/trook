using Microsoft.AspNetCore.Mvc;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/player")]
public class PlayerController(PlayerService playerService, ILogger<PlayerController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJobs([FromRoute] Guid profileId)
    {
        logger.LogInformation("Getting all driver jobs");
        var jobs = await playerService.GetAllJobsForProfile(profileId);
        return Ok(jobs);
    }
}