using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.DTOs;
using TrookApi.Util;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class GarageService(TrookDbContext db, ILogger<GarageService> logger)
{
    public async Task<Result<GarageData>> ExtractGaragesFromFile(Guid profileId, SiiBinaryFile file)
    {
        logger.LogInformation("Extracting garage info from file...");
        var garages = new List<Garage>();
        var truckMap = new Dictionary<string, Garage>();
        var trailerMap = new Dictionary<string, Garage>();
        var driverMap = new Dictionary<string, Garage>();
        var profitLogMap = new Dictionary<string, Garage>();

        var returnVal = new GarageData(garages, driverMap, truckMap, trailerMap, profitLogMap);

        try
        {
            var allGarages = file.GetDataByStructureName("garage");

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
                garages.Add(garage);
                
                // get child block lists
                foreach (var blockId in garageBlock.GetArray<BlockId>("vehicles"))
                {
                    if (!blockId.IsEmpty)
                        truckMap[blockId.Key] = garage;
                }
                foreach (var blockId in garageBlock.GetArray<BlockId>("trailers"))
                {
                    if (!blockId.IsEmpty)
                        trailerMap[blockId.Key] = garage;
                }
                foreach (var blockId in garageBlock.GetArray<BlockId>("drivers"))
                {
                    if (!blockId.IsEmpty)
                        driverMap[blockId.Key] = garage;
                }

                var profitLogId = garageBlock.GetValue<BlockId>("profit_log");
                if (!profitLogId.IsEmpty)
                    profitLogMap[profitLogId.Key] = garage;
            }
        }
        catch (Exception e)
        {
            var errMsg = "An error occurred extracting garages from file";
            logger.LogError(e, errMsg);
            return Result<GarageData>.Failure(errMsg, e);
        }

        var msg = "Successfully extracted garages from file";
        logger.LogInformation(msg);
        return Result<GarageData>.Success(returnVal, msg);
    }
}