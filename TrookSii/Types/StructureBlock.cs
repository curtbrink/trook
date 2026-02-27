namespace TrookSii.Types;

public class StructureBlock
{
    public uint Id { get; init; }
    
    public string Name { get; init; }
    
    public IList<ValueDefinition> Values { get; init; }
    
    public IDictionary<uint, string>? OrdinalStrings { get; init; }
}