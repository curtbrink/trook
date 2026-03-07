using Microsoft.EntityFrameworkCore;
using TrookApi.Database.Entities;

namespace TrookApi.Database;

public class TrookDbContext(DbContextOptions<TrookDbContext> options) : DbContext(options)
{
    public DbSet<ProcessedFile> ProcessedFiles { get; set; }
}