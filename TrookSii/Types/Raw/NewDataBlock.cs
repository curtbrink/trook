namespace TrookSii.Types.Raw;

public class NewDataBlock(BlockId blockId, StructureBlock structure, IList<(ValueDefinition, dynamic)> data)
{
    public BlockId Id { get; } = blockId;

    public StructureBlock Structure { get; } = structure;

    private Dictionary<string, dynamic> _data = data.ToDictionary(t => t.Item1.Name, t => t.Item2);

    public dynamic this[string key] => _data[key];
}