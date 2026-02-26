namespace TrookSii.Types;

public class DataBlock
{
    public uint StructureId { get; init; }
    
    public BlockId BlockId { get; init; }
    
    public IEnumerable<byte[]> Data { get; init; }
}