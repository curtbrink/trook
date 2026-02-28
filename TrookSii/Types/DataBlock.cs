namespace TrookSii.Types;

public class DataBlock
{
    public uint StructureId { get; init; }
    
    public string BlockId { get; init; }
    
    public IList<(ValueDefinition, dynamic)> Data { get; init; }
}