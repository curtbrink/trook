namespace TrookSii.Types;

public class BlockId
{
    public byte Length { get; init; }
    
    public IList<ulong> Parts { get; init; }
}