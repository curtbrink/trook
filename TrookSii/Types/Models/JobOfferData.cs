using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class JobOfferData(BlockId blockId): BaseSii(blockId)
{
    // Sii properties
    
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
    public BlockId CargoId { get; set; }
    
    [Sii("company_truck")]
    public string CompanyTruck { get; set; } = "";
    
    [Sii("trailer_variant")]
    public BlockId TrailerVariantId { get; set; }
    
    [Sii("trailer_definition")]
    public BlockId TrailerDefinitionId { get; set; }
    
    [Sii("units_count")]
    public uint UnitsCount { get; set; }
    
    [Sii("fill_ratio")]
    public float FillRatio { get; set; }

    [Sii("trailer_place")]
    public float[][] TrailerPlace { get; set; } = [];
    
    // Relations
    
    public BaseSii? Cargo { get; set; }
    public BaseSii? TrailerVariant { get; set; }
    public BaseSii? TrailerDefinition { get; set; }

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
        Cargo = GetBlockById(CargoId, blockMap);
        TrailerVariant = GetBlockById(TrailerVariantId, blockMap);
        TrailerDefinition = GetBlockById(TrailerDefinitionId, blockMap);
    }
}