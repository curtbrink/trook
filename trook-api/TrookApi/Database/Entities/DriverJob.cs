using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("driver_jobs")]
public class DriverJob
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    [MaxLength(128)]
    [Column("driver_id")]
    public required string DriverId { get; set; } // future state: relation on Driver table
    
    [Column("day_completed")]
    public uint DayCompleted { get; set; }
    
    [Column("is_empty")]
    public bool IsEmpty { get; set; }
    
    [Column("revenue")]
    public long Revenue { get; set; }
    
    [Column("wage")]
    public long Wage { get; set; }
    
    [Column("maintenance")]
    public long Maintenance { get; set; }
    
    [Column("fuel")]
    public long Fuel { get; set; }
    
    [Column("distance")]
    public uint Distance { get; set; }
    
    [MaxLength(128)]
    [Column("cargo_type")]
    public string? CargoType { get; set; }
    
    [Column("cargo_size")]
    public uint? CargoSize { get; set; }
    
    [MaxLength(128)]
    [Column("source_city")]
    public required string SourceCity { get; set; }
    
    [MaxLength(128)]
    [Column("source_company")]
    public string? SourceCompany { get; set; }
    
    [MaxLength(128)]
    [Column("dest_city")]
    public required string DestinationCity { get; set; }
    
    [MaxLength(128)]
    [Column("dest_company")]
    public string? DestinationCompany { get; set; }
}