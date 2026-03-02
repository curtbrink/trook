using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class ProfitLogEntry(BlockId blockId) : BaseSii(blockId)
{
    [Sii("revenue")]
    public long Revenue { get; set; }
    
    [Sii("wage")]
    public long Wage { get; set; }
    
    [Sii("maintenance")]
    public long Maintenance { get; set; }
    
    [Sii("fuel")]
    public long Fuel { get; set; }
    
    [Sii("distance")]
    public uint Distance { get; set; }
    
    [Sii("distance_on_job")]
    public bool DistanceOnJob { get; set; }
    
    [Sii("cargo_count")]
    public uint CargoCount { get; set; }

    [Sii("cargo")]
    public EncodedString Cargo { get; set; }

    [Sii("source_city")]
    public EncodedString SourceCity { get; set; }
    
    [Sii("source_company")]
    public EncodedString SourceCompany { get; set; }
    
    [Sii("destination_city")]
    public EncodedString DestinationCity { get; set; }
    
    [Sii("destination_company")]
    public EncodedString DestinationCompany { get; set; }
    
    [Sii("timestamp_day")]
    public uint TimestampDay { get; set; }

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
    }
}