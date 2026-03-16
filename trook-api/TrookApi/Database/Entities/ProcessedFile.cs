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
    public Guid Id { get; init; }

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; init; }

    [MaxLength(128)]
    [Column("file_name")]
    public required string FileName { get; init; }

    [MaxLength(16)]
    [Column("file_hash")]
    public required byte[] FileHash { get; init; }

    [Column("is_success")]
    public bool IsSuccess { get; init; }

    [MaxLength(512)]
    [Column("error_message")]
    public string? ErrorMessage { get; init; }
}