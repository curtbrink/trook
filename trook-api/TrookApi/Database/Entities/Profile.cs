using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrookApi.Database.Entities;

[Table(("profiles"))]
public class Profile
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
    [Column("name")]
    public required string Name { get; init; }
}