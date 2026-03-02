using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public abstract class BaseSii(BlockId blockId)
{
    public BlockId BlockId => blockId;

    public abstract void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap);

    protected static T? GetTypedBlockById<T>(BlockId blockId, IDictionary<string, BaseSii> lookup) where T : BaseSii
    {
        var b = GetBlockById(blockId, lookup);
        return b as T;
    }

    protected static T[] GetTypedBlocksById<T>(BlockId[] blockIds, IDictionary<string, BaseSii> lookup)
        where T : BaseSii
    {
        return blockIds.Select(bId => GetTypedBlockById<T>(bId, lookup)).OfType<T>().ToArray();
    }
    
    protected static BaseSii? GetBlockById(BlockId blockId, IDictionary<string, BaseSii> lookup)
    {
        if (blockId.IsEmpty) return null;
        
        var found = lookup.TryGetValue(blockId.Key, out var block);
        return found ? block : null;
    }

    protected static BaseSii[] GetBlocksById(BlockId[] blockIds, IDictionary<string, BaseSii> lookup)
    {
        return blockIds.Select(bId => GetBlockById(bId, lookup)).OfType<BaseSii>().ToArray();
    }
}