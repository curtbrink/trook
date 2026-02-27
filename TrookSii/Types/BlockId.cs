using TrookSii.Stream.Extensions;

namespace TrookSii.Types;

public class BlockId
{
    public byte Length { get; init; }
    
    public IList<ulong> Parts { get; init; }

    public string DisplayName => Length == 255
        ? $"_nameless.{Parts[0]:X}"
        : string.Join(".", Parts.Select(SiiStreamTypeExtensions.DecodeEncodedString));
}