using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("garages")]
public class Garage
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
    
    [ForeignKey(nameof(Profile))]
    [Column("profile_id")]
    public Guid ProfileId { get; init; }
    
    [MaxLength(128)]
    [Column("city")]
    public required string City { get; init; }
    
    [Column("status")]
    public uint Status { get; init; } // 0 = unowned, others maybe an enum?
    
    [Column("productivity")]
    public float Productivity { get; init; }
}