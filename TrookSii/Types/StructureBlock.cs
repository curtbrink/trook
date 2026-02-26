namespace TrookSii.Types;

public class StructureBlock
{
    public uint Id { get; init; }
    
    public string Name { get; init; }
    
    public IEnumerable<ValueDefinition> Values { get; init; }
}