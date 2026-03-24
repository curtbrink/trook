using Microsoft.EntityFrameworkCore;
using TrookApi.Database.Entities;

namespace TrookApi.Database;

public class TrookDbContext(DbContextOptions<TrookDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles { get; set; }
    
    public DbSet<ProcessedFile> ProcessedFiles { get; set; }

    public DbSet<DriverJob> DriverJobs { get; set; }
    
    public DbSet<Player> Players { get; set; }
    
    public DbSet<PlayerJob> PlayerJobs { get; set; }
    
    public DbSet<Garage> Garages { get; set; }
    
    public DbSet<Driver> Drivers { get; set; }
    
    public DbSet<LocalizationEntry> LocalizationEntries { get; set; }
}