using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;
using TrookSii.Types.Blocks;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Decoders;

public static class DataBlockDecoder
{
    public static bool Decode(SiiStream sii, IDictionary<uint, StructureBlock> structureBlocks,
        out DataBlock? block, ILogger<SiiStream>? logger = null)
    {
        var structId = sii.ReadUInt32();
        StructureBlock structure;
        try
        {
            structure = structureBlocks[structId];
        }
        catch (KeyNotFoundException e)
        {
            logger?.LogInformation($"Invalid structure block id: {structId}");
            block = null;
            return false;
        }

        logger?.LogInformation($"BEGIN: Data block for structure (id={structId}, name={structure.Name})");

        var dataBlockId = sii.ReadDataBlockId();
        if (dataBlockId.IsEmpty)
            throw new InvalidOperationException("Data block has invalid id!");

        logger?.LogInformation($"==> id: {dataBlockId}");

        var dataValues = new List<(ValueDefinition, SiiValue)>();

        foreach (var vd in structure)
        {
            var vdValue = sii.GetValueForDefinition(vd, structure, logger);
            dataValues.Add((vd, vdValue));
        }

        block = new DataBlock(dataBlockId, structure, dataValues);

        return true;
    }
}