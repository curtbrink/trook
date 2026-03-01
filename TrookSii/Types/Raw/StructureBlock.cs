namespace TrookSii.Types.Raw;

public class StructureBlock
{
    public uint Id { get; init; }
    
    public required string Name { get; init; }

    public IList<ValueDefinition> Values { get; init; } = [];
    
    public IDictionary<uint, string>? OrdinalStrings { get; init; }
}