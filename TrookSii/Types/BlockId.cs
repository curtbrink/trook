namespace TrookSii.Types;

public class BlockId
{
    public byte Length { get; init; }
    
    public IEnumerable<ulong> Parts { get; init; }
}