using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table("localized_strings")]
public class LocalizationEntry
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; init; }
    
    [ForeignKey(nameof(Profile))]
    [Column("profile_id")]
    public Guid ProfileId { get; set; }
    
    [MaxLength(128)]
    [Column("key")]
    public required string Key { get; init; }
    
    [MaxLength(128)]
    [Column("localized")]
    public required string Localized { get; set; }
}