using Microsoft.AspNetCore.Mvc;
using TrookApi.Services;

namespace TrookApi.Controllers;

[ApiController]
[Route("/api/v1/profiles/{profileId:guid}/garages")]
public class GarageController(GarageService garageService, ILogger<GarageController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGarages([FromRoute] Guid profileId)
    {
        logger.LogInformation("Getting all driver jobs");
        var garages = await garageService.GetAllGaragesForProfile(profileId);
        return Ok(garages);
    }
}