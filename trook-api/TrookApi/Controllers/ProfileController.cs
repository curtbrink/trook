using Microsoft.AspNetCore.Mvc;
using TrookApi.DTOs;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles")]
public class ProfileController(
    ProfileService profileService,
    FileService fileService,
    ILogger<ProfileController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        logger.LogInformation("Getting all profiles");
        var profiles = await profileService.GetAllProfiles();
        return Ok(profiles);
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> CreateProfileFromJson([FromBody] ProfileCreateRequest profile)
    {
        logger.LogInformation("Creating profile from json request");
        var createdProfile = await profileService.CreateProfile(profile);
        return Ok(createdProfile);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProfileFromFile([FromForm] IFormFile file)
    {
        logger.LogInformation("Creating profile from sii file");
        var saveFileResult = await fileService.SaveFileFromFormAsync(file);
        if (saveFileResult.SiiFile is null || saveFileResult.ProcessedFile is null)
        {
            return BadRequest();
        }

        var profile = await profileService.CreateProfileFromSii(saveFileResult.SiiFile, saveFileResult.ProcessedFile);
        return Ok(profile);
    }
}