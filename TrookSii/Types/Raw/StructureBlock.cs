namespace TrookSii.Types.Raw;

public class StructureBlock(
    uint id,
    string name,
    IList<ValueDefinition> valueDefinitions,
    IDictionary<uint, string>? ordinalStrings = null)
{
    private readonly IDictionary<string, ValueDefinition> _valueDefinitionsByName =
        valueDefinitions.ToDictionary(vd => vd.Name, vd => vd);
    
    public uint Id { get; } = id;

    public string Name { get; } = name;

    public ValueDefinition GetDefinition(string name) => _valueDefinitionsByName[name];

    public ValueDefinition GetDefinition(int idx) => valueDefinitions[idx];

    public int DefinitionCount => valueDefinitions.Count;

    public IEnumerator<ValueDefinition> GetEnumerator() => valueDefinitions.GetEnumerator();

    public string? GetOrdinalString(uint idx) => ordinalStrings?[idx];
}