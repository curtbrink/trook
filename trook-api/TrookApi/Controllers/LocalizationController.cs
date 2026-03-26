using Microsoft.AspNetCore.Mvc;
using TrookApi.DTOs;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/localization")]
public class LocalizationController(LocalizationService localizationService, ILogger<LocalizationController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStrings([FromRoute] Guid profileId)
    {
        logger.LogInformation("Getting all pretty strings for profile");
        var strings = await localizationService.GetAllStringsForProfile(profileId);
        return Ok(strings);
    }

    [HttpPost]
    public async Task<IActionResult> AddString([FromRoute] Guid profileId, [FromBody] CreateStringRequest request)
    {
        logger.LogInformation("Creating new localization entry");
        var entry = await localizationService.CreateString(profileId, request);
        return Ok(entry);
    }

    [HttpPut("{stringId:guid}")]
    public async Task<IActionResult> UpdateString([FromRoute] Guid profileId, [FromRoute] Guid stringId,
        [FromBody] UpdateStringRequest request)
    {
        logger.LogInformation("Updating value for localization entry");
        var entry = await localizationService.UpdateString(stringId, profileId, request);
        return Ok(entry);
    }
}