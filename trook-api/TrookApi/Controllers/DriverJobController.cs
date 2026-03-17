using Microsoft.AspNetCore.Mvc;
using TrookApi.Services;
using TrookSii.Types.Raw;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/jobs")]
public class DriverJobController(DriverJobService driverJobService, FileService fileService, ILogger<DriverJobController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJobs([FromRoute] Guid profileId)
    {
        logger.LogInformation("Getting all driver jobs");
        var jobs = await driverJobService.GetAllJobsForProfile(profileId);
        return Ok(jobs);
    }
    
    [HttpPost]
    public async Task<IActionResult> ReadJobsFromFile([FromRoute] Guid profileId, [FromForm] IFormFile file)
    {
        logger.LogInformation("Reading driver jobs from sii file");
        var saveFileResult = await fileService.SaveFileFromFormAsync(file, profileId);
        if (saveFileResult.SiiFile is not SiiBinaryFile sbf || saveFileResult.ProcessedFile is null)
        {
            return BadRequest();
        }

        var newJobs = await driverJobService.ExtractDriverJobs(profileId, sbf);

        logger.LogInformation("Finished processing file");
        return Ok(newJobs);
    }
}