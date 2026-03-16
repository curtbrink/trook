using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
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
    public async Task<IActionResult> IngestFile([FromForm] IFormFile file)
    {
        logger.LogInformation("Processing file!");
        
        var l = file.Length;
        var bytes = new byte[l];
        var writeStream = new MemoryStream(bytes);
        using var stream = file.OpenReadStream();
        await stream.CopyToAsync(writeStream);

        var processed = await fileService.SaveFileAsync(file.FileName, bytes);
        if (processed is not SiiBinaryFile sbf)
        {
            return BadRequest();
        }

        await driverJobService.ExtractDriverJobs(sbf);

        logger.LogInformation("Finished processing file");
        return Ok();
    }
}