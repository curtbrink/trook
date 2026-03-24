using Microsoft.EntityFrameworkCore;
using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookApi.Util;
using TrookSii.Types.Blocks;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class DriverService(TrookDbContext db, ILogger<DriverService> logger)
{
    public async Task<List<DriverJob>> GetAllJobsForProfile(Guid profileId)
    {
        var profileDriverIds = await db.Drivers.Where(d => d.ProfileId == profileId).Select(d => d.Id).ToListAsync();
        var allJobs = await db.DriverJobs.Where(dj => profileDriverIds.Contains(dj.DriverId)).ToListAsync();
        return allJobs;
    }

    public async Task<Result<List<Driver>>> ExtractDrivers(Guid profileId, SiiBinaryFile file, Dictionary<string, Garage> driverLookup)
    {
        logger.LogInformation("Extracting driver info from file...");
        var drivers = new List<Driver>();
        
        try
        {
            var allDrivers = file.GetDataByStructureName("driver_ai");

            foreach (var driverBlock in allDrivers)
            {
                var garageFound = driverLookup.TryGetValue(driverBlock.Id.Key, out var garage);
                if (!garageFound || garage is null) continue; // not player-hired

                var driver = new Driver
                {
                    DriverKey = driverBlock.Id.Key,
                    GarageId = garage.Id,
                    ProfileId = profileId
                };
                drivers.Add(driver);
            }
        }
        catch (Exception e)
        {
            var errMsg = "An error occurred extracting drivers from file";
            logger.LogError(e, errMsg);
            return Result<List<Driver>>.Failure(errMsg, e);
        }

        var msg = "Successfully extracted drivers from file";
        logger.LogInformation(msg);
        return Result<List<Driver>>.Success(drivers, msg);
    }
    
    public async Task<Result<List<DriverJob>>> ExtractDriverJobs(SiiBinaryFile file, List<Driver> drivers)
    {
        logger.LogInformation("Extracting driver jobs from file...");
        var jobsToSave = new List<DriverJob>();

        try
        {
            foreach (var driver in drivers)
            {
                var driverBlock = file.GetData(driver.DriverKey);
                var profitLog = file.GetData(driverBlock.GetValue<BlockId>("profit_log").Key);
                var entryIds = profitLog.GetArray<BlockId>("stats_data");
                jobsToSave.AddRange(entryIds.Select(pfe => file.GetData(pfe.Key))
                    .Select(entry => MapJob(entry, driverBlock, driver.Id)));
            }
        }
        catch (Exception e)
        {
            var errMsg = "An error occurred extracting driver jobs from file";
            logger.LogError(e, errMsg);
            return Result<List<DriverJob>>.Failure(errMsg, e);
        }

        var msg = "Successfully extracted driver jobs";
        logger.LogInformation(msg);
        return Result<List<DriverJob>>.Success(jobsToSave, msg);
    }

    private static DriverJob MapJob(DataBlock entry, DataBlock driver, Guid driverId)
    {
        var isRevenue = entry.GetValue<bool>("distance_on_job");
        // fill out required fields
        var job = new DriverJob
        {
            DriverId = driverId,
            DayCompleted = entry.GetValue<uint>("timestamp_day"),
            IsEmpty = !isRevenue,
            Revenue = entry.GetValue<long>("revenue"),
            Wage = entry.GetValue<long>("wage"),
            Maintenance = entry.GetValue<long>("maintenance"),
            Fuel = entry.GetValue<long>("fuel"),
            Distance = entry.GetValue<uint>("distance"),
            SourceCity = entry.GetValue<string>("source_city"),
            DestinationCity = entry.GetValue<string>("destination_city")
        };

        if (isRevenue)
        {
            // fill out cargo and company deets
            job.CargoType = entry.GetValue<string>("cargo");
            job.CargoSize = entry.GetValue<uint>("cargo_count");
            job.SourceCompany = entry.GetValue<string>("source_company");
            job.DestinationCompany = entry.GetValue<string>("destination_company");
        }

        return job;
    }
}