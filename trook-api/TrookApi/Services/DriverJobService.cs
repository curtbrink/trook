using TrookApi.Database;
using TrookApi.Database.Entities;
using TrookSii.Types.Blocks;
using TrookSii.Types.Raw;

namespace TrookApi.Services;

public class DriverJobService(TrookDbContext db, ILogger<DriverJobService> logger)
{
    public async Task ExtractDriverJobs(SiiBinaryFile file)
    {
        var jobsToSave = new List<DriverJob>();
        var drivers = file.GetDataByStructureName("driver_ai");
        foreach (var driver in drivers)
        {
            var profitLog = file.GetData(driver.GetValue<BlockId>("profit_log").Key);
            var entryIds = profitLog.GetArray<BlockId>("stats_data");
            foreach (var pfe in entryIds)
            {
                var entry = file.GetData(pfe.Key);
                var job = MapJob(entry, driver);
                jobsToSave.Add(job);
            }
        }
        
        await db.AddRangeAsync(jobsToSave);
        await db.SaveChangesAsync();
    }

    private DriverJob MapJob(DataBlock entry, DataBlock driver)
    {
        var isRevenue = entry.GetValue<bool>("distance_on_job");
        // fill out required fields
        var job = new DriverJob
        {
            DriverId = driver.Id.Key,
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