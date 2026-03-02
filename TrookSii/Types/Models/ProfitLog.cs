using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class ProfitLog(BlockId blockId) : BaseSii(blockId)
{
    // Sii properties
    
    [Sii("stats_data")]
    public BlockId[] StatsDataBlockIds { get; set; } = [];

    [Sii("acc_distance_on_job")]
    public uint TotalDistanceOnJob { get; set; }
    
    [Sii("acc_distance_free")]
    public uint TotalDistanceOffJob { get; set; }
    
    [Sii("history_age")]
    public uint HistoryAge { get; set; }
    
    // Relations

    public ProfitLogEntry[] ProfitLogEntries { get; set; } = [];

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
        ProfitLogEntries = GetTypedBlocksById<ProfitLogEntry>(StatsDataBlockIds, blockMap);
    }
}