using TrookSii.Types.Models;

namespace TrookSii.Types.Raw;

public class DataBlock(BlockId blockId) : BaseSii(blockId)
{
    public uint StructureId { get; init; }

    public IList<(ValueDefinition, dynamic)> Data { get; init; } = [];
    
    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
    }
}