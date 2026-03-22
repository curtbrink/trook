using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;
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
    
    public async Task<PlayerExtractResult> ExtractPlayer(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Extracting player and delivery logs from file...");
        
        // update player first
        var playerEntity = await UpsertPlayer(profileId, file);
        
        var jobsToSave = new List<PlayerJob>();

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
                PlayerId = playerEntity.Id,
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

        logger.LogInformation("Player jobs extracted; saving to database...");
        await db.AddRangeAsync(jobsToSave);
        await db.SaveChangesAsync();
        logger.LogInformation("Successfully saved player jobs");

        return new PlayerExtractResult(playerEntity, jobsToSave);
    }

    private async Task<Player> UpsertPlayer(Guid profileId, SiiBinaryFile file)
    {
        var econBlock = file.GetDataByStructureName("economy").First();
        var totalDistance = econBlock.GetValue<uint>("total_distance");
        var playerBlock = file.GetDataByStructureName("player").First();
        var hqCity = playerBlock.GetValue<string>("hq_city");

        var player = await db.Players.FirstOrDefaultAsync(p => p.ProfileId == profileId);
        if (player is null)
        {
            player = new Player { ProfileId = profileId, HeadquartersCity = hqCity, TotalDistance = totalDistance };
            await db.Players.AddAsync(player);
        }
        else
        {
            // update - TODO add more
            player.HeadquartersCity = hqCity;
            player.TotalDistance = totalDistance;
        }
        
        await db.SaveChangesAsync();
        return player;
    }
}