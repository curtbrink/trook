using Microsoft.AspNetCore.Mvc;
using TrookApi.Services;
using TrookSii.Types.Raw;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/files")]
public class FileController(FileService fileService, DataIngestionService dataService, ILogger<FileController> logger)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> IngestDataFromFile([FromRoute] Guid profileId, [FromForm] IFormFile file)
    {
        logger.LogInformation("Reading driver jobs from sii file");
        var saveFileResult = await fileService.SaveFileFromFormAsync(file, profileId);
        if (saveFileResult.SiiFile is not SiiBinaryFile sbf || saveFileResult.ProcessedFile is null)
        {
            return BadRequest();
        }

        await dataService.IngestFile(profileId, sbf);
        
        return Ok();
    }
}