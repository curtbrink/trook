using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("player_jobs")]
public class PlayerJob
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
    
    [ForeignKey(nameof(Player))]
    [Column("player_id")]
    public Guid PlayerId { get; init; }
    
    [Column("is_quick")]
    public bool IsQuickJob { get; init; }
    
    [Column("started_at")]
    public int StartedAt { get; init; }
    
    [Column("finished_at")]
    public int FinishedAt { get; init; }
    
    [MaxLength(128)]
    [Column("source_city")]
    public required string SourceCity { get; init; }
    
    [MaxLength(128)]
    [Column("source_company")]
    public required string SourceCompany { get; set; }
    
    [MaxLength(128)]
    [Column("dest_city")]
    public required string DestinationCity { get; init; }
    
    [MaxLength(128)]
    [Column("dest_company")]
    public required string DestinationCompany { get; set; }
    
    [MaxLength(128)]
    [Column("cargo_type")]
    public required string CargoType { get; set; }
    
    [Column("cargo_size")]
    public int CargoSize { get; set; }
    
    [Column("cargo_weight")]
    public float CargoWeight { get; set; }
    
    [Column("base_distance")]
    public int BaseDistance { get; set; }
    
    // [Column("base_eta")]
    // public int TimeAllotted { get; set; }
    
    [Column("base_revenue")]
    public int BaseRevenue { get; set; }
    
    [Column("real_revenue")]
    public int Revenue { get; init; }
    
    [Column("real_distance")]
    public int Distance { get; init; }
    
    [Column("real_xp")]
    public int Xp { get; init; }
    
    // [Column("xp_penalty")]
    // public int XpPenalty { get; init; }
    
    [Column("parking_level")]
    public int ParkingLevel { get; init; }
}