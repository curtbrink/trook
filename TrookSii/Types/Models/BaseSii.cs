using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public abstract class BaseSii(BlockId blockId)
{
    public BlockId BlockId => blockId;
}