using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class Company(BlockId blockId) : BaseSii(blockId)
{
    [Sii("permanent_data")]
    public BlockId PermanentData { get; set; }
    
    [Sii("delivered_trailer")]
    public BlockId DeliveredTrailer { get; set; }

    [Sii("delivered_pos")]
    public float[][] DeliveredPosition { get; set; } = [];

    [Sii("job_offer")]
    public BlockId[] JobOffers { get; set; } = [];
    
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
}