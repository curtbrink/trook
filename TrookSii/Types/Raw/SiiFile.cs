using TrookSii.Types.Blocks;

namespace TrookSii.Types.Raw;

public abstract class SiiFile(uint signature)
{
    public const uint BsiiHeaderSignature = 0x49495342;
    public const uint PlainHeaderSignature = 0x4E696953;

    public uint Signature { get; } = signature;
}

public class SiiTextFile(IList<PlainBlock> blocks) : SiiFile(BsiiHeaderSignature)
{
    public IDictionary<string, PlainBlock> Data { get; } = blocks.ToDictionary(pb => pb.Id.Key, pb => pb);
}

public class SiiBinaryFile(uint version, IList<StructureBlock> structureBlocks, IList<DataBlock> dataBlocks)
    : SiiFile(PlainHeaderSignature)
{
    public uint Version { get; } = version;

    public int StructureCount { get; } = structureBlocks.Count;

    public int DataCount { get; } = dataBlocks.Count;

    private readonly Dictionary<uint, StructureBlock> _structureDict =
        structureBlocks.ToDictionary(sb => sb.Id, sb => sb);

    private readonly Dictionary<string, DataBlock> _dataDict = dataBlocks.ToDictionary(db => db.Id.Key, db => db);

    public StructureBlock GetStructure(uint id) => _structureDict[id];

    public StructureBlock GetStructureByName(string name) => structureBlocks.First(s => s.Name == name);

    public DataBlock GetData(string id) => _dataDict[id];

    public IList<DataBlock> GetDataByStructureId(uint id) => dataBlocks.Where(db => db.Structure.Id == id).ToList();

    public IList<DataBlock> GetDataByStructureName(string name)
    {
        var sb = GetStructureByName(name);
        return dataBlocks.Where(db => db.Structure.Id == sb.Id).ToList();
    }
}