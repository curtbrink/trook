using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class EconomyEvent(BlockId blockId) : BaseSii(blockId)
{
    // Sii properties
    
    [Sii("time")]
    public uint Time { get; set; }
    
    [Sii("unit_link")]
    public BlockId UnitLinkId { get; set; }
    
    [Sii("param")]
    public uint Param { get; set; }
    
    // Relations
    
    public BaseSii? UnitLink { get; set; }

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
        UnitLink = GetBlockById(UnitLinkId, blockMap);
    }
}