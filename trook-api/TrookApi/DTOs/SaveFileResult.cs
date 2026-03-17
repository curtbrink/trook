using TrookApi.Database.Entities;
using TrookSii.Types.Raw;

namespace TrookApi.DTOs;

public record SaveFileResult(SiiFile? SiiFile, ProcessedFile? ProcessedFile);