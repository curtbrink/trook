using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.DTOs;
using TrookApi.Services;
using TrookSii.Types.Raw;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/admin")]
public class AdminController(FileService fileService, DriverJobService driverJobService, TrookDbContext db, ILogger<AdminController> logger) : ControllerBase
{
    [HttpPost("clear-all")]
    public async Task<IActionResult> ClearData()
    {
        logger.LogInformation("Clearing all data!");
        await db.DriverJobs.ExecuteDeleteAsync();
        await db.ProcessedFiles.ExecuteDeleteAsync();
        return Ok();
    }

    [HttpPost("ingest-file")]
    public async Task<IActionResult> IngestFile(IngestFileRequest request)
    {
        logger.LogInformation("Processing file!");
        var file = await fileService.ReadAndSaveFileAsync(request.FilePath);
        if (file is not SiiBinaryFile sbf)
        {
            return BadRequest();
        }

        await driverJobService.ExtractDriverJobs(sbf);

        logger.LogInformation("Finished processing file");
        return Ok();
    }
}