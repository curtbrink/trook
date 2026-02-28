using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;
using TrookSii.Types;

namespace TrookSii;

public static class SiiDecoder
{
    // general file structure according to SII_Decrypt github docs

    // a File consists of:
    // - 4 byte signature "BSII"
    // - 4 byte version
    // - one or more Blocks

    // a Block consists of:
    // - 4 byte block type
    // - payload

    // if block type is 0x0, it is a Structure Block:
    // - 1 byte validity bool (0x0 or 0x1)
    // - 4 byte structure id
    // - string (var) structure name
    // - zero or more Values
    // - 4 byte invalid value type (0x0) marking the end of the block

    // if block type is not 0x0, it is a Data Block:
    // - BlockId
    // - zero or more values that align with the value types defined in the corresponding Structure Block

    private const uint BsiiHeader = 0x49495342;

    public static SiiFile DecodeSii(byte[] data, ILogger<SiiStream>? logger = null)
    {
        var sii = new SiiStream(ref data);

        // verify header
        var header = sii.ReadUInt32();
        if (header != BsiiHeader)
        {
            throw new InvalidOperationException("Unexpected file signature");
        }

        var version = sii.ReadUInt32();
        logger?.LogInformation($"File version: {version}");

        var validBlock = true;
        var structureBlocks = new Dictionary<uint, StructureBlock>();
        var dataBlocks = new List<DataBlock>();
        while (validBlock)
        {
            // peek at block type
            var blockType = sii.ReadUInt32(true);
            if (blockType == 0)
            {
                // structure block
                validBlock = DecodeStructureBlock(sii, out var block, logger);
                if (block != null && validBlock) structureBlocks[block.Id] = block;
            }
            else
            {
                // data block
                validBlock = DecodeDataBlock(sii, structureBlocks, out var block, logger);
                if (block != null && validBlock) dataBlocks.Add(block);
            }
        }

        return new SiiFile
        {
            Signature = header,
            Version = version,
            Data = dataBlocks,
            Structures = structureBlocks
        };
    }

    private static bool DecodeStructureBlock(SiiStream sii, out StructureBlock? block,
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
                ordinals = DecodeOrdinalStringList(sii, valueName, logger);
            }

            valueTypes.Add(new ValueDefinition { TypeId = valueTypeId, Name = valueName });
            // logger?.LogInformation($"==> field: {valueName} (type = 0x{valueTypeId:X})");
            valueTypeId = sii.ReadUInt32();
        }

        logger?.LogInformation("END:   Structure block");

        block = new StructureBlock
        {
            Id = structureId,
            Name = name,
            Values = valueTypes,
            OrdinalStrings = ordinals
        };
        return true;
    }

    private static IDictionary<uint, string> DecodeOrdinalStringList(SiiStream sii, string name,
        ILogger<SiiStream>? logger = null)
    {
        var numStrings = sii.ReadUInt32();
        var ordinalDict = new Dictionary<uint, string>();
        for (uint i = 0; i < numStrings; i++)
        {
            var ord = sii.ReadUInt32();
            var s = sii.ReadString();
            logger?.LogInformation($"{name}[{ord}]: {s}");
            ordinalDict[ord] = s;
        }

        return ordinalDict;
    }

    private static bool DecodeDataBlock(SiiStream sii, IDictionary<uint, StructureBlock> structureBlocks,
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
        logger?.LogInformation($"==> id: 0x{dataBlockId.Parts.First():X}");

        var dataValues = new List<(ValueDefinition, dynamic)>();

        foreach (var vd in structure.Values)
        {
            dynamic vdValue;
            logger?.LogInformation($"==> field: {vd.Name} (type = 0x{vd.TypeId:X})");
            switch (vd.TypeId)
            {
                case 0x01:
                    vdValue = sii.ReadString();
                    logger?.LogInformation($"====> string: {vdValue}");
                    break;
                case 0x02:
                    vdValue = sii.ReadStringArray();
                    foreach (var s in vdValue)
                        logger?.LogInformation($"====> string: {s}");
                    break;
                case 0x03:
                    vdValue = sii.ReadEncodedString();
                    logger?.LogInformation($"====> enc string: {vdValue}");
                    break;
                case 0x04:
                    vdValue = sii.ReadEncodedStringArray();
                    foreach (var s in vdValue)
                        logger?.LogInformation($"====> enc string: {s}");
                    break;
                case 0x05:
                    vdValue = sii.ReadFloat();
                    logger?.LogInformation($"====> float: {vdValue}");
                    break;
                case 0x06:
                    vdValue = sii.ReadFloatArray();
                    foreach (var sf in vdValue)
                        logger?.LogInformation($"====> float: {sf}");
                    break;
                case 0x07:
                    vdValue = sii.ReadVec2S();
                    logger?.LogInformation($"====> vec2floats: [{vdValue[0]}, {vdValue[1]}]");
                    break;
                case 0x09:
                    vdValue = sii.ReadVec3S();
                    logger?.LogInformation($"====> vec3floats: [{vdValue[0]}, {vdValue[1]}, {vdValue[2]}]");
                    break;
                case 0x11:
                    vdValue = sii.ReadVec3I();
                    foreach (var v in vdValue)
                        logger?.LogInformation($"====> int: {v}");
                    break;
                case 0x12:
                    vdValue = sii.ReadVec3IArray();
                    foreach (var vec3 in vdValue)
                        logger?.LogInformation($"====> vec3: [{vec3[0]}, {vec3[1]}, {vec3[2]}]");
                    break;
                case 0x18:
                    vdValue = sii.ReadVec4SArray();
                    foreach (var vec4A in vdValue)
                    {
                        logger?.LogInformation("====> vec4s array:");
                        foreach (var vec4 in vec4A)
                            logger?.LogInformation($"======> float: {vec4}");
                    }

                    break;
                case 0x19:
                    vdValue = sii.ReadVec8S();
                    foreach (var weirdFloat in vdValue)
                        logger?.LogInformation($"====> biased float: {weirdFloat}");
                    break;
                case 0x1a:
                    vdValue = sii.ReadVec8SArray();
                    foreach (var wfa in vdValue)
                    {
                        logger?.LogInformation($"====> weird float array:");
                        foreach (var wfa2 in wfa)
                            logger?.LogInformation($"======> weird float: {wfa2}");
                    }

                    break;
                case 0x25:
                    vdValue = sii.ReadInt32();
                    logger?.LogInformation($"====> int: {vdValue}");
                    break;
                case 0x26:
                    vdValue = sii.ReadInt32Array();
                    foreach (var s in vdValue)
                        logger?.LogInformation($"====> int: {s}");
                    break;
                case 0x27:
                    vdValue = sii.ReadUInt32();
                    logger?.LogInformation($"====> uint: {vdValue}");
                    break;
                case 0x28:
                    vdValue = sii.ReadUInt32Array();
                    foreach (var nsv in vdValue)
                        logger?.LogInformation($"====> uint: {vdValue}");
                    break;
                case 0x2b:
                    vdValue = sii.ReadUInt16();
                    logger?.LogInformation($"====> ushort: {vdValue}");
                    break;
                case 0x2c:
                    vdValue = sii.ReadUInt16Array();
                    foreach (var us in vdValue)
                        logger?.LogInformation($"====> ushort: {us}");
                    break;
                case 0x2f:
                    vdValue = sii.ReadUInt32();
                    logger?.LogInformation($"====> uint: {vdValue}");
                    break;
                case 0x31:
                    vdValue = sii.ReadInt64();
                    logger?.LogInformation($"====> long: {vdValue}");
                    break;
                case 0x32:
                    vdValue = sii.ReadInt64Array();
                    foreach (var slv in vdValue)
                        logger?.LogInformation($"====> long: {slv}");
                    break;
                case 0x33:
                    vdValue = sii.ReadUInt64();
                    logger?.LogInformation($"====> ulong: {vdValue}");
                    break;
                case 0x34:
                    vdValue = sii.ReadUInt64Array();
                    foreach (var ui64 in vdValue)
                        logger?.LogInformation($"====> ulong: {ui64}");
                    break;
                case 0x35:
                    vdValue = sii.ReadBool();
                    logger?.LogInformation($"====> bool: {vdValue}");
                    break;
                case 0x36:
                    vdValue = sii.ReadBoolArray();
                    foreach (var b in vdValue)
                        logger?.LogInformation($"====> bool: {b}");
                    break;
                case 0x37:
                    // ordinal strings on the comeback
                    var ordIdx = sii.ReadUInt32();
                    vdValue = structure.OrdinalStrings?[ordIdx] ?? "";
                    logger?.LogInformation($"====> ordinal string: {vdValue}");
                    break;
                case 0x39:
                case 0x3b:
                case 0x3d:
                    vdValue = sii.ReadDataBlockId();
                    foreach (var b in vdValue.Parts)
                        logger?.LogInformation($"====> block id: 0x{b:X}");
                    break;
                case 0x3a:
                case 0x3c:
                    vdValue = sii.ReadDataBlockIdArray();
                    foreach (var b in vdValue)
                    {
                        logger?.LogInformation("====> block list:");
                        foreach (var p in b.Parts)
                        {
                            logger?.LogInformation($"======> id: 0x{p:X}");
                        }
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Unknown data type! 0x{vd.TypeId:X}");
            }

            dataValues.Add((vd, vdValue));
        }

        block = new DataBlock
        {
            StructureId = structure.Id,
            BlockId = dataBlockId,
            Data = dataValues
        };
        return true;
    }
}