using TrookApi.Database.Entities;

namespace TrookApi.DTOs;

public record PlayerExtractResult(Player Player, List<PlayerJob> PlayerJobs);