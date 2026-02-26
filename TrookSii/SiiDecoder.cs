using System.Text;
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

    public static SiiFile DecodeSii(byte[] data)
    {
        var sii = new SiiStream(ref data);
        
        // verify header
        var header = sii.ReadUInt32();
        if (header != BsiiHeader)
        {
            throw new InvalidOperationException("Unexpected file signature");
        }

        var version = sii.ReadUInt32();
        Console.WriteLine($"File version: {version}");

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
                validBlock = DecodeStructureBlock(sii, out var block);
                if (block != null && validBlock) structureBlocks[block.Id] = block;
            }
            else
            {
                // data block
                validBlock = DecodeDataBlock(sii, structureBlocks, out var block);
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

    private static bool DecodeStructureBlock(SiiStream sii, out StructureBlock? block)
    {
        var blockType = sii.ReadUInt32();
        if (blockType != 0)
        {
            throw new InvalidOperationException($"Not a structure block! expected type=0, got type={blockType}");
        }

        var validity = sii.ReadBoolByte();
        if (!validity)
        {
            block = null;
            return false;
        }

        Console.WriteLine("BEGIN: Structure block");

        var structureId = sii.ReadUInt32();
        Console.WriteLine($"==> id: {structureId}");

        var name = sii.ReadString();
        Console.WriteLine($"==> name: {name}");

        var valueTypes = new List<ValueDefinition>();
        IDictionary<uint, string>? ordinals = null;
        var valueTypeId = sii.ReadUInt32();
        while (valueTypeId != 0)
        {
            var valueName = sii.ReadString();
            if (valueTypeId == 0x37)
            {
                Console.WriteLine("==> ordinal strings:");
                ordinals = DecodeOrdinalStringList(sii, valueName);
            }
            valueTypes.Add(new ValueDefinition { TypeId = valueTypeId, Name = valueName });
            Console.WriteLine($"==> field: {valueName} (type = 0x{valueTypeId:X})");
            valueTypeId = sii.ReadUInt32();
        }

        Console.WriteLine("END:   Structure block");

        block = new StructureBlock
        {
            Id = structureId,
            Name = name,
            Values = valueTypes,
            OrdinalStrings = ordinals
        };
        return true;
    }

    private static IDictionary<uint, string> DecodeOrdinalStringList(SiiStream sii, string name)
    {
        var numStrings = sii.ReadUInt32();
        var ordinalDict = new Dictionary<uint, string>();
        for (uint i = 0; i < numStrings; i++)
        {
            var ord = sii.ReadUInt32();
            var s = sii.ReadString();
            Console.WriteLine($"{name}[{ord}]: {s}");
            ordinalDict[ord] = s;
        }

        return ordinalDict;
    }

    private static bool DecodeDataBlock(SiiStream sii, IDictionary<uint, StructureBlock> structureBlocks,
        out DataBlock? block)
    {
        var structId = sii.ReadUInt32();
        StructureBlock structure;
        try
        {
            structure = structureBlocks[structId];
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine($"Invalid structure block id: {structId}");
            block = null;
            return false;
        }

        Console.WriteLine($"BEGIN: Data block for structure (id={structId}, name={structure.Name})");

        var dataBlockId = sii.ReadDataBlockId();
        Console.WriteLine($"==> id: 0x{dataBlockId.Parts.First():X}");

        block = null;
        return false;
    }
}