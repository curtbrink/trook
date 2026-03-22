using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class DataIngestionService(
    PlayerService playerService,
    DriverJobService driverJobService,
    GarageService garageService,
    ILogger<DataIngestionService> logger)
{
    public async Task IngestFile(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Processing save game data from file");

        // PLAYER AND PLAYER JOBS
        var playerResult = await playerService.ExtractPlayer(profileId, file);

        // DRIVER JOBS
        var newJobs = await driverJobService.ExtractDriverJobs(profileId, file);

        // GARAGES - do we want to do this first-ish? we can map drivers, trucks, etc to garages via id
        await garageService.ExtractGaragesFromFile(profileId, file);

        // TODO
        // drivers
        // trucks
        // other events...
        // also deduping is still on the list

        logger.LogInformation("Finished processing file");
    }
}