using System.Text;
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

        var validity = sii.ReadBool();
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

        var allValuesParseable = true;

        foreach (var vd in structure.Values)
        {
            Console.WriteLine($"==> field: {vd.Name} (type = 0x{vd.TypeId:X})");
            switch (vd.TypeId)
            {
                case 0x01:
                    var str = sii.ReadString();
                    Console.WriteLine($"====> string: {str}");
                    break;
                case 0x02:
                    var strs = sii.ReadStringArray();
                    foreach (var s in strs)
                        Console.WriteLine($"====> string: {s}");
                    break;
                case 0x03:
                    var encStr = sii.ReadEncodedString();
                    Console.WriteLine($"====> enc string: {encStr}");
                    break;
                case 0x04:
                    var encStrs = sii.ReadEncodedStringArray();
                    foreach (var s in encStrs)
                        Console.WriteLine($"====> enc string: {s}");
                    break;
                case 0x05:
                    var f = sii.ReadFloat();
                    Console.WriteLine($"====> float: {f}");
                    break;
                case 0x06:
                    var fs = sii.ReadFloatArray();
                    foreach (var sf in fs)
                        Console.WriteLine($"====> float: {sf}");
                    break;
                case 0x07:
                    var twoFloats = sii.ReadVec2S();
                    Console.WriteLine($"====> vec2floats: [{twoFloats[0]}, {twoFloats[1]}]");
                    break;
                case 0x09:
                    var threeFloats = sii.ReadVec3S();
                    Console.WriteLine($"====> vec3floats: [{threeFloats[0]}, {threeFloats[1]}, {threeFloats[2]}]");
                    break;
                case 0x11:
                    var v3 = sii.ReadVec3I();
                    foreach (var v in v3)
                        Console.WriteLine($"====> int: {v}");
                    break;
                case 0x12:
                    var vec3a = sii.ReadVec3IArray();
                    foreach (var vec3 in vec3a)
                        Console.WriteLine($"====> vec3: [{vec3[0]}, {vec3[1]}, {vec3[2]}]");
                    break;
                case 0x18:
                    var vec4sa = sii.ReadVec4SArray();
                    foreach (var vec4a in vec4sa)
                    {
                        Console.WriteLine("====> vec4s array:");
                        foreach (var vec4 in vec4a)
                            Console.WriteLine($"======> float: {vec4}");
                    }

                    break;
                case 0x19:
                    var weirdFloats = sii.ReadVec8S();
                    foreach (var weirdFloat in weirdFloats)
                        Console.WriteLine($"====> biased float: {weirdFloat}");
                    break;
                case 0x1a:
                    var weirdFloatArray = sii.ReadVec8SArray();
                    foreach (var wfa in weirdFloatArray)
                    {
                        Console.WriteLine($"====> weird float array:");
                        foreach (var wfa2 in wfa)
                            Console.WriteLine($"======> weird float: {wfa2}");
                    }

                    break;
                case 0x25:
                    var i = sii.ReadInt32();
                    Console.WriteLine($"====> int: {i}");
                    break;
                case 0x26:
                    var sia = sii.ReadInt32Array();
                    foreach (var s in sia)
                        Console.WriteLine($"====> int: {s}");
                    break;
                case 0x27:
                    var n = sii.ReadUInt32();
                    Console.WriteLine($"====> uint: {n}");
                    break;
                case 0x28:
                    var ns = sii.ReadUInt32Array();
                    foreach (var nsv in ns)
                        Console.WriteLine($"====> uint: {nsv}");
                    break;
                case 0x2b:
                    var singleUs = sii.ReadUInt16();
                    Console.WriteLine($"====> ushort: {singleUs}");
                    break;
                case 0x2c:
                    var usa = sii.ReadUInt16Array();
                    foreach (var us in usa)
                        Console.WriteLine($"====> ushort: {us}");
                    break;
                case 0x2f:
                    var ui = sii.ReadUInt32();
                    Console.WriteLine($"====> uint: {ui}");
                    break;
                case 0x31:
                    var l = sii.ReadInt64();
                    Console.WriteLine($"====> long: {l}");
                    break;
                case 0x32:
                    var sl = sii.ReadInt64Array();
                    foreach (var slv in sl)
                        Console.WriteLine($"====> long: {slv}");
                    break;
                case 0x33:
                    var ul = sii.ReadUInt64();
                    Console.WriteLine($"====> ulong: {ul}");
                    break;
                case 0x34:
                    var uls = sii.ReadUInt64Array();
                    foreach (var ui64 in uls)
                        Console.WriteLine($"====> ulong: {ui64}");
                    break;
                case 0x35:
                    var bo = sii.ReadBool();
                    Console.WriteLine($"====> bool: {bo}");
                    break;
                case 0x36:
                    var boa = sii.ReadBoolArray();
                    foreach (var b in boa)
                        Console.WriteLine($"====> bool: {b}");
                    break;
                case 0x37:
                    // ordinal strings on the comeback
                    var ordIdx = sii.ReadUInt32();
                    var ordS = structure.OrdinalStrings?[ordIdx] ?? "";
                    Console.WriteLine($"====> ordinal string: {ordS}");
                    break;
                case 0x39:
                case 0x3b:
                case 0x3d:
                    var bId = sii.ReadDataBlockId();
                    foreach (var b in bId.Parts)
                        Console.WriteLine($"====> block id: 0x{b:X}");
                    break;
                case 0x3a:
                case 0x3c:
                    var bIds = sii.ReadDataBlockIdArray();
                    foreach (var b in bIds)
                    {
                        Console.WriteLine("====> block list:");
                        foreach (var p in b.Parts)
                        {
                            Console.WriteLine($"======> id: 0x{p:X}");
                        }
                    }

                    break;
                default:
                    allValuesParseable = false;
                    break;
            }

            if (!allValuesParseable)
            {
                break;
            }
        }

        if (!allValuesParseable)
        {
            Console.WriteLine("Missing types :(");
        }
        
        block = null;
        return allValuesParseable;
    }
}