using System.Text;

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

    public static async Task DecodeSii(byte[] data)
    {
        var idx = 0;
        
        // verify header
        var header = BitConverter.ToUInt32(data, idx);
        idx += 4;
        if (header != BsiiHeader)
        {
            throw new InvalidOperationException("Unexpected file signature");
        }

        var version = BitConverter.ToUInt32(data, idx);
        idx += 4;
        Console.WriteLine($"File version: {version}");

        var validBlock = true;
        while (validBlock)
        {
            validBlock = ParseBlock(ref data, ref idx);
        }
    }

    private static bool ParseBlock(ref byte[] data, ref int idx)
    {
        var blockType = BitConverter.ToUInt32(data, idx);
        idx += 4;

        if (blockType == 0)
        {
            Console.WriteLine("Structure block!");
            var validityByte = BitConverter.ToBoolean(data, idx);
            idx++;

            if (!validityByte)
            {
                Console.WriteLine("Invalid structure block - must be the end");
                return false;
            }

            var structureId = BitConverter.ToUInt32(data, idx);
            idx += 4;
            Console.WriteLine($"structure id: {structureId}");

            var nameLength = BitConverter.ToUInt32(data, idx);
            idx += 4;
            Console.WriteLine($"structure name is {nameLength} bytes");

            var nameBytes = data[idx..(idx += (int)nameLength)];
            var name = Encoding.UTF8.GetString(nameBytes);
            Console.WriteLine($"structure name: {name}");
        }

        return false;
    }
}