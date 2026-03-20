using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("players")]
public class Player
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
    
    [Column("total_distance")]
    public long TotalDistance { get; set; }
    
    [MaxLength(128)]
    [Column("hq_city")]
    public required string HeadquartersCity { get; set; }
}