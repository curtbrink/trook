using TrookSii.Types.Mappings;

namespace TrookSii.Types.Models;

public class ProfitLogEntry
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
    public string Cargo { get; set; } = "";

    [Sii("source_city")]
    public string SourceCity { get; set; } = "";
    
    [Sii("source_company")]
    public string SourceCompany { get; set; } = "";
    
    [Sii("destination_city")]
    public string DestinationCity { get; set; } = "";
    
    [Sii("destination_company")]
    public string DestinationCompany { get; set; } = "";
    
    [Sii("timestamp_day")]
    public uint TimestampDay { get; set; }
}