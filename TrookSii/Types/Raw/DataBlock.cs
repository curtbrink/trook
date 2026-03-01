namespace TrookSii.Types.Raw;

public class DataBlock
{
    public uint StructureId { get; init; }
    
    public required string BlockId { get; init; }

    public IList<(ValueDefinition, dynamic)> Data { get; init; } = [];
}