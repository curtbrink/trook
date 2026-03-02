using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class EconomyEvent(BlockId blockId) : BaseSii(blockId)
{
    [Sii("time")]
    public uint Time { get; set; }
    
    [Sii("unit_link")]
    public BlockId UnitLink { get; set; }
    
    [Sii("param")]
    public uint Param { get; set; }
}