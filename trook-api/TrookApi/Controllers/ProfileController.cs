using Microsoft.AspNetCore.Mvc;
using TrookApi.DTOs;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles")]
public class ProfileController(ProfileService profileService, ILogger<ProfileController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        logger.LogInformation("Getting all profiles");
        var profiles = await profileService.GetAllProfiles();
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] ProfileCreateRequest profile)
    {
        logger.LogInformation("Creating profile");
        var createdProfile = await profileService.CreateProfile(profile);
        return Ok(createdProfile);
    }
}