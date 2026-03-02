using TrookSii.Types.Models;

namespace TrookSii.Types.Raw;

public class SiiFile
{
    public uint Signature { get; init; }
    
    public uint Version { get; init; }

    public IList<StructureBlock> Structures { get; init; } = [];
    
    // strongly typed data blocks

    public IList<ProfitLog> ProfitLogBlocks { get; init; } = [];

    public IList<ProfitLogEntry> ProfitLogEntryBlocks { get; init; } = [];

    public IList<AiDriver> AiDriverBlocks { get; init; } = [];

    public IList<Company> CompanyBlocks { get; init; } = [];

    public IList<JobOfferData> JobOfferDataBlocks { get; init; } = [];
    
    public IList<EconomyEvent> EconomyEventBlocks { get; init; } = [];

    // fallback "generic" data blocks that don't have types mapped yet
    public IList<DataBlock> Data { get; init; } = [];
}