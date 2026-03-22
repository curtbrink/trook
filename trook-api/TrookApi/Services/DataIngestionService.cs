using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class DataIngestionService(PlayerService playerService, DriverJobService driverJobService, ILogger<DataIngestionService> logger)
{
    public async Task IngestFile(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Processing save game data from file");
        
        // PLAYER AND PLAYER JOBS
        var playerResult = await playerService.ExtractPlayer(profileId, file);
        
        // DRIVER JOBS
        var newJobs = await driverJobService.ExtractDriverJobs(profileId, file);
        
        // TODO
        // garages
        // drivers
        // trucks
        // other events...
        // also deduping is still on the list

        logger.LogInformation("Finished processing file");
    }
}