using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class JobOfferData(BlockId blockId): BaseSii(blockId)
{
    [Sii("target")]
    public string Target { get; set; } = "";
    
    [Sii("expiration_time")]
    public uint ExpirationTime { get; set; }
    
    [Sii("urgency")]
    public uint Urgency { get; set; }
    
    [Sii("shortest_distance_km")]
    public ushort ShortestDistanceKm { get; set; }
    
    [Sii("ferry_time")]
    public ushort FerryTime { get; set; }
    
    [Sii("ferry_price")]
    public ushort FerryPrice { get; set; }
    
    [Sii("cargo")]
    public BlockId Cargo { get; set; }
    
    [Sii("company_truck")]
    public string CompanyTruck { get; set; } = "";
    
    [Sii("trailer_variant")]
    public BlockId TrailerVariant { get; set; }
    
    [Sii("trailer_definition")]
    public BlockId TrailerDefinition { get; set; }
    
    [Sii("units_count")]
    public uint UnitsCount { get; set; }
    
    [Sii("fill_ratio")]
    public float FillRatio { get; set; }

    [Sii("trailer_place")]
    public float[][] TrailerPlace { get; set; } = [];
}