using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class GarageService(TrookDbContext db, ILogger<GarageService> logger)
{
    public async Task ExtractGaragesFromFile(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Extracting garage info from file...");

        // todo for now we slam everything in the db.
        // but maybe we want to limit to player-owned in the future.
        // we definitely want to dedupe.
        var allGarages = file.GetDataByStructureName("garage");

        var toSave = new List<Garage>();
        foreach (var garageBlock in allGarages)
        {
            // parse city key - it comes from the block id
            var splitId = garageBlock.Id.Key.Split(".");
            if (splitId.Length != 2)
            {
                logger.LogWarning("Couldn't identify garage block {Id}; is it formatted correctly?",
                    garageBlock.Id.Key);
                continue;
            }

            var garage = new Garage
            {
                ProfileId = profileId,
                City = splitId[1],
                Status = garageBlock.GetValue<uint>("status"),
                Productivity = garageBlock.GetValue<float>("productivity")
            };
            toSave.Add(garage);
        }
        
        await db.Garages.AddRangeAsync(toSave);
        await db.SaveChangesAsync();
    }
    
    // save game contains ALL garages, owned and unowned.
    // unowned appear to have status != 0
    // status 3 = large?
    // status 2 = medium?
    // status 6 = tiny?
    
    // garage struct:
    // - status (uint)
    // - vehicles (blockid[])
    // - drivers (blockid[])
    // - trailers (blockid[])
    // - profit_log (blockid)
    // - productivity (float)
}