using TrookApi.Database;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class DataIngestionService(
    TrookDbContext db,
    PlayerService playerService,
    DriverService driverService,
    GarageService garageService,
    ILogger<DataIngestionService> logger)
{
    public async Task IngestFile(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Processing save game data from file");

        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var garageResult = await garageService.ExtractGaragesFromFile(profileId, file);
            if (!garageResult.IsSuccess || garageResult.Data is null)
                throw new Exception("Failed to extract garages", garageResult.Errors.FirstOrDefault());
            await db.AddRangeAsync(garageResult.Data.Entities);
            await db.SaveChangesAsync();

            var playerResult = await playerService.ExtractPlayer(profileId, file, garageResult.Data.DriverIdMap);
            if (!playerResult.IsSuccess || playerResult.Data is null)
                throw new Exception("Failed to extract player", playerResult.Errors.FirstOrDefault());
            await db.AddAsync(playerResult.Data);
            await db.SaveChangesAsync();

            var playerJobResult = await playerService.ExtractPlayerJobs(file, playerResult.Data.Id);
            if (!playerJobResult.IsSuccess || playerJobResult.Data is null)
                throw new Exception("Failed to extract jobs", playerJobResult.Errors.FirstOrDefault());
            await db.AddRangeAsync(playerJobResult.Data);
            await db.SaveChangesAsync();

            var driverResult = await driverService.ExtractDrivers(profileId, file, garageResult.Data.DriverIdMap);
            if (!driverResult.IsSuccess || driverResult.Data is null)
                throw new Exception("Failed to extract drivers", driverResult.Errors.FirstOrDefault());
            await db.AddRangeAsync(driverResult.Data);
            await db.SaveChangesAsync();

            var driverJobsResult = await driverService.ExtractDriverJobs(file, driverResult.Data);
            if (!driverJobsResult.IsSuccess || driverJobsResult.Data is null)
                throw new Exception("Failed to extract driver jobs", driverJobsResult.Errors.FirstOrDefault());
            await db.AddRangeAsync(driverJobsResult.Data);
            await db.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error processing file");
            await transaction.RollbackAsync();
        }
        // todo
        // other events...
        // also deduping is still on the list

        logger.LogInformation("Finished processing file");
    }
}