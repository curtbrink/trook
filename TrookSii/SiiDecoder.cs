using System.Reflection;
using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;
using TrookSii.Types;
using TrookSii.Types.Mappings;
using TrookSii.Types.Models;
using TrookSii.Types.Raw;

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
        var structureBlockLookup = new Dictionary<uint, StructureBlock>();
        var structureBlocks = new List<StructureBlock>();
        var dataBlocks = new List<NewDataBlock>();
        while (validBlock)
        {
            // peek at block type
            var blockType = sii.ReadUInt32(true);
            if (blockType == 0)
            {
                // structure block
                validBlock = DecodeStructureBlock(sii, out var block, logger);
                if (block != null && validBlock)
                {
                    structureBlockLookup[block.Id] = block;
                    structureBlocks.Add(block);
                }
            }
            else
            {
                // data block
                validBlock = DecodeDataBlock(sii, structureBlockLookup, out var block, logger);
                if (block != null && validBlock)
                {
                    // add lookup entry and then add to list
                    dataBlocks.Add(block);
                }
            }
        }

        return new SiiFile(header, version, structureBlocks, dataBlocks);
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

        block = new StructureBlock(structureId, name, valueTypes, ordinals);
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
        out NewDataBlock? block, ILogger<SiiStream>? logger = null)
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

        block = new NewDataBlock(dataBlockId, structure, dataValues);

        return true;
    }
}