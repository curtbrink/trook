using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("drivers")]
public class Driver
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    [ForeignKey(nameof(Profile))]
    [Column("profile_id")]
    public Guid ProfileId { get; init; }
    
    [ForeignKey(nameof(Garage))]
    [Column("garage_id")]
    public Guid GarageId { get; init; }
    
    [MaxLength(128)]
    [Column("driver_key")]
    public required string DriverKey { get; init; }
}