using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("driver_jobs")]
public class DriverJob
{
    [Key]
    [Column("id")]
    public Guid Id { get; init; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; init; }
    
    [MaxLength(128)]
    [Column("driver_id")]
    public required string DriverId { get; init; } // future state: relation on Driver table
    
    [Column("day_completed")]
    public uint DayCompleted { get; init; }
    
    [Column("is_empty")]
    public bool IsEmpty { get; init; }
    
    [Column("revenue")]
    public long Revenue { get; init; }
    
    [Column("wage")]
    public long Wage { get; init; }
    
    [Column("maintenance")]
    public long Maintenance { get; init; }
    
    [Column("fuel")]
    public long Fuel { get; init; }
    
    [Column("distance")]
    public uint Distance { get; init; }
    
    [MaxLength(128)]
    [Column("cargo_type")]
    public string? CargoType { get; set; }
    
    [Column("cargo_size")]
    public uint? CargoSize { get; set; }
    
    [MaxLength(128)]
    [Column("source_city")]
    public required string SourceCity { get; init; }
    
    [MaxLength(128)]
    [Column("source_company")]
    public string? SourceCompany { get; set; }
    
    [MaxLength(128)]
    [Column("dest_city")]
    public required string DestinationCity { get; init; }
    
    [MaxLength(128)]
    [Column("dest_company")]
    public string? DestinationCompany { get; set; }
}