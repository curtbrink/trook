using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.Util;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class PlayerService(TrookDbContext db, ILogger<PlayerService> logger)
{
    public async Task<List<PlayerJob>> GetAllJobsForProfile(Guid profileId)
    {
        var player = await db.Players.FirstOrDefaultAsync(p => p.ProfileId == profileId);
        if (player is null)
        {
            logger.LogError($"Player for profile id {profileId} not found");
            return [];
        }

        var allJobs = await db.PlayerJobs.Where(dj => dj.PlayerId == player.Id).ToListAsync();
        return allJobs;
    }
    
    public async Task<Result<Player>> ExtractPlayer(Guid profileId, SiiBinaryFile file, Dictionary<string, Garage> driverIdMap)
    {
        logger.LogInformation("Extracting player from file...");

        Player playerToSave;
        try
        {
            // update player first
            var econBlock = file.GetDataByStructureName("economy").First();
            var totalDistance = econBlock.GetValue<uint>("total_distance");
            var playerBlock = file.GetDataByStructureName("player").First();
            var hqCity = playerBlock.GetValue<string>("hq_city");

            var driverPlayerBlock = file.GetDataByStructureName("driver_player").First();
            var driverKey = driverPlayerBlock.Id.Key;
            var garageId = driverIdMap[driverKey].Id;

            playerToSave = new Player
            {
                ProfileId = profileId,
                GarageId = garageId,
                DriverKey = driverKey,
                HeadquartersCity = hqCity,
                TotalDistance = totalDistance
            };
            var existingPlayer = await db.Players.FirstOrDefaultAsync(p => p.ProfileId == profileId);
            if (existingPlayer is not null)
            {
                // update existing player
                playerToSave.Id = existingPlayer.Id;
                playerToSave.CreatedAt = existingPlayer.CreatedAt;
            }
        }
        catch (Exception e)
        {
            var errMsg = "An error occurred extracting player from file";
            logger.LogError(e, errMsg);
            return Result<Player>.Failure(errMsg, e);
        }

        var msg = "Successfully extracted player from file";
        logger.LogInformation(msg);
        return Result<Player>.Success(playerToSave, msg);
    }

    public async Task<Result<List<PlayerJob>>> ExtractPlayerJobs(SiiBinaryFile file, Guid playerId)
    {
        logger.LogInformation("Extracting player jobs from file...");
        var jobsToSave = new List<PlayerJob>();

        try
        {
            var deliveryLog = file.GetDataByStructureName("delivery_log").First();
            var entryIds = deliveryLog.GetArray<BlockId>("entries");
            foreach (var entryId in entryIds)
            {
                var entry = file.GetData(entryId.Key);
                var entryValues = entry.GetArray<string>("params");

                // delivery log entries are just a flat list of strings...

                // if not a job, skip it
                if (entryValues[18] == "freerm") continue;

                // split some keys first - source and dest companies are "company.volatile.<name>.<city>"
                var source = entryValues[1].Split(".");
                var dest = entryValues[2].Split(".");
                // cargo is "cargo.<key>"
                var cargo = entryValues[3].Split(".");

                var job = new PlayerJob
                {
                    PlayerId = playerId,
                    IsQuickJob = entryValues[18] == "quick",
                    StartedAt = int.Parse(entryValues[15]),
                    FinishedAt = int.Parse(entryValues[0]),
                    SourceCity = source[3],
                    SourceCompany = source[2],
                    DestinationCity = dest[3],
                    DestinationCompany = dest[2],
                    CargoType = cargo[1],
                    CargoSize = int.Parse(entryValues[23]),
                    CargoWeight = float.Parse(entryValues[22]),
                    BaseDistance = int.Parse(entryValues[17]),
                    Distance = int.Parse(entryValues[6]),
                    Xp = int.Parse(entryValues[4]),
                    // XpPenalty = int.Parse(entryValues[8]),
                    BaseRevenue = int.Parse(entryValues[13]),
                    Revenue = int.Parse(entryValues[5]),
                    ParkingLevel = int.Parse(entryValues[11]),
                };
                jobsToSave.Add(job);
            }
        }
        catch (Exception e)
        {
            var errMsg = "An error occurred extracting player jobs from file";
            logger.LogError(e, errMsg);
            return Result<List<PlayerJob>>.Failure(errMsg, e);
        }

        var msg = "Successfully extracted player jobs from file";
        logger.LogInformation(msg);
        return Result<List<PlayerJob>>.Success(jobsToSave, msg);
    }
}