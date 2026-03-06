using TrookSii.Types.Models;

namespace TrookSii.Types.Raw;

public class SiiFile(uint signature, uint version, IList<StructureBlock> structureBlocks, IList<NewDataBlock> dataBlocks)
{
    public uint Signature { get; } = signature;

    public uint Version { get; } = version;
    
    public int StructureCount { get; } = structureBlocks.Count;

    public int DataCount { get; } = dataBlocks.Count;

    private readonly Dictionary<uint, StructureBlock> _structureDict = structureBlocks.ToDictionary(sb => sb.Id, sb => sb);

    private readonly Dictionary<string, NewDataBlock> _dataDict = dataBlocks.ToDictionary(db => db.Id.Key, db => db);

    public StructureBlock GetStructure(uint id) => _structureDict[id];

    public NewDataBlock GetData(string id) => _dataDict[id];
}