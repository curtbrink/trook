using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class Company(BlockId blockId) : BaseSii(blockId)
{
    // Sii properties
    
    [Sii("permanent_data")]
    public BlockId PermanentDataId { get; set; }
    
    [Sii("delivered_trailer")]
    public BlockId DeliveredTrailerId { get; set; }

    [Sii("delivered_pos")]
    public float[][] DeliveredPosition { get; set; } = [];

    [Sii("job_offer")]
    public BlockId[] JobOfferIds { get; set; } = [];
    
    [Sii("cargo_offer_seeds")]
    public uint[] CargoOfferSeeds { get; set; } = [];
    
    [Sii("discovered")]
    public bool Discovered { get; set; }
    
    [Sii("reserved_trailer_slot")]
    public uint ReservedTrailerSlot { get; set; }
    
    [Sii("state")]
    public uint State { get; set; }
    
    [Sii("state_change_time")]
    public uint StateChangeTime { get; set; }
    
    // Relations
    
    public BaseSii? PermanentData { get; set; }
    public BaseSii? DeliveredTrailer { get; set; }
    public JobOfferData[] JobOffers { get; set; } = [];

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
        PermanentData = GetBlockById(PermanentDataId, blockMap);
        DeliveredTrailer = GetBlockById(DeliveredTrailerId, blockMap);
        JobOffers = GetTypedBlocksById<JobOfferData>(JobOfferIds, blockMap);
    }
}