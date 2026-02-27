namespace TrookSii.Types;

public class SiiFile
{
    public uint Signature { get; init; }
    
    public uint Version { get; init; }
    
    public IDictionary<uint, StructureBlock> Structures { get; init; }
    
    public IList<DataBlock> Data { get; init; }
}