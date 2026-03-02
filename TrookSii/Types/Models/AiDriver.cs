using TrookSii.Types.Mappings;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Models;

public class AiDriver(BlockId blockId) : BaseSii(blockId)
{
    // Sii properties
    
    [Sii("adr")]
    public uint Adr { get; set; }
    
    [Sii("long_dist")]
    public uint LongDist { get; set; }
    
    [Sii("heavy")]
    public uint Heavy { get; set; }
    
    [Sii("fragile")]
    public uint Fragile { get; set; }
    
    [Sii("urgent")]
    public uint Urgent { get; set; }
    
    [Sii("mechanical")]
    public uint Mechanical { get; set; }
    
    [Sii("hometown")]
    public EncodedString Hometown { get; set; }
    
    [Sii("current_city")]
    public EncodedString CurrentCity { get; set; }
    
    [Sii("state")]
    public uint State { get; set; }
    
    [Sii("on_duty_timer")]
    public int OnDutyTimer { get; set; }
    
    [Sii("extra_maintenance")]
    public long ExtraMaintenance { get; set; }
    
    [Sii("driver_job")]
    public BlockId DriverJobId { get; set; }
    
    [Sii("experience_points")]
    public uint ExperiencePoints { get; set; }
    
    [Sii("training_policy")]
    public uint TrainingPolicy { get; set; }
    
    [Sii("adopted_truck")]
    public BlockId AdoptedTruckId { get; set; }
    
    [Sii("assigned_truck")]
    public BlockId AssignedTruckId { get; set; }
    
    [Sii("assigned_truck_efficiency")]
    public float AssignedTruckEfficiency { get; set; }
    
    [Sii("assigned_truck_axle_count")]
    public uint AssignedTruckAxleCount { get; set; }
    
    [Sii("assigned_truck_mass")]
    public float AssignedTruckMass { get; set; }
    
    [Sii("slot_truck_efficiency")]
    public float SlotTruckEfficiency { get; set; }
    
    [Sii("slot_truck_axle_count")]
    public uint SlotTruckAxleCount { get; set; }
    
    [Sii("slot_truck_mass")]
    public float SlotTruckMass { get; set; }
    
    [Sii("adopted_trailer")]
    public BlockId AdoptedTrailerId { get; set; }
    
    [Sii("assigned_trailer")]
    public BlockId AssignedTrailerId { get; set; }
    
    [Sii("old_hometown")]
    public EncodedString OldHometown { get; set; }
    
    [Sii("profit_log")]
    public BlockId ProfitLogId { get; set; }
    
    // Relations
    
    public BaseSii? DriverJob { get; set; }
    public BaseSii? AdoptedTruck { get; set; }
    public BaseSii? AssignedTruck { get; set; }
    public BaseSii? AdoptedTrailer { get; set; }
    public BaseSii? AssignedTrailer { get; set; }
    public ProfitLog? ProfitLog { get; set; }

    public override void MapRelatedBlocks(IDictionary<string, BaseSii> blockMap)
    {
        DriverJob = GetBlockById(DriverJobId, blockMap);
        AdoptedTruck = GetBlockById(AdoptedTruckId, blockMap);
        AssignedTruck = GetBlockById(AssignedTruckId, blockMap);
        AdoptedTrailer = GetBlockById(AdoptedTrailerId, blockMap);
        AssignedTrailer = GetBlockById(AssignedTrailerId, blockMap);
        ProfitLog = GetTypedBlockById<ProfitLog>(ProfitLogId, blockMap);
    }
}