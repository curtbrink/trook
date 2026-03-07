using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TrookApi.Database.Entities;

[Index(nameof(FileHash), nameof(IsSuccess))]
[Table("processed_files")]
public class ProcessedFile
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
    [Column("file_name")]
    public required string FileName { get; set; }
    
    [MaxLength(16)]
    [Column("file_hash")]
    public required byte[] FileHash { get; set; }
    
    [Column("is_success")]
    public bool IsSuccess { get; set; }

    [MaxLength(512)]
    [Column("error_message")]
    public string? ErrorMessage { get; set; }
}