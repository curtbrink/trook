using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;
using TrookSii.Types.Blocks;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Decoders;

public static class StructureBlockDecoder
{
    public static bool Decode(SiiStream sii, out StructureBlock? block,
        ILogger<SiiStream>? logger = null)
    {
        var blockType = sii.ReadUInt32();
        if (blockType != 0)
        {
            throw new InvalidOperationException($"Not a structure block! expected type=0, got type={blockType}");
        }

        var validity = sii.ReadBool();
        if (!validity)
        {
            block = null;
            return false;
        }

        logger?.LogInformation("BEGIN: Structure block");

        var structureId = sii.ReadUInt32();
        logger?.LogInformation($"==> id: {structureId}");

        var name = sii.ReadString();
        logger?.LogInformation($"==> name: {name}");

        var valueTypes = new List<ValueDefinition>();
        IDictionary<uint, string>? ordinals = null;
        var valueTypeId = sii.ReadUInt32();
        while (valueTypeId != 0)
        {
            var valueName = sii.ReadString();
            if (valueTypeId == 0x37)
            {
                logger?.LogInformation("==> ordinal strings:");
                ordinals = OrdinalStringDecoder.Decode(sii, valueName, logger);
            }

            valueTypes.Add(new ValueDefinition { TypeId = valueTypeId, Name = valueName });
            // logger?.LogInformation($"==> field: {valueName} (type = 0x{valueTypeId:X})");
            valueTypeId = sii.ReadUInt32();
        }

        logger?.LogInformation("END:   Structure block");

        block = new StructureBlock(structureId, name, valueTypes, ordinals);
        return true;
    }
}