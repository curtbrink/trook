using System.Text;
using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Types.Blocks;
using TrookSii.Types.Decoders;
using TrookSii.Types.Decoders.PlainText;
using TrookSii.Types.Raw;

namespace TrookSii;

public static class SiiDecoder
{
    public static SiiFile DecodeSii(byte[] data, ILogger<SiiStream>? logger = null)
    {
        var sii = new SiiStream(ref data);

        // verify header
        var header = sii.ReadUInt32();

        return header switch
        {
            SiiFile.BsiiHeaderSignature => DecodeBinarySii(sii, logger),
            SiiFile.PlainHeaderSignature => DecodePlainSii(data, logger),
            _ => throw new InvalidOperationException("Unexpected file signature!")
        };
    }

    private static SiiBinaryFile DecodeBinarySii(SiiStream sii, ILogger<SiiStream>? logger = null)
    {
        var version = sii.ReadUInt32();
        logger?.LogInformation($"File version: {version}");

        var validBlock = true;
        var structureBlockLookup = new Dictionary<uint, StructureBlock>();
        var structureBlocks = new List<StructureBlock>();
        var dataBlocks = new List<DataBlock>();
        while (validBlock)
        {
            // peek at block type
            var blockType = sii.ReadUInt32(true);
            if (blockType == 0)
            {
                // structure block
                validBlock = StructureBlockDecoder.Decode(sii, out var block, logger);
                if (block != null && validBlock)
                {
                    structureBlockLookup[block.Id] = block;
                    structureBlocks.Add(block);
                }
            }
            else
            {
                // data block
                validBlock = DataBlockDecoder.Decode(sii, structureBlockLookup, out var block, logger);
                if (block != null && validBlock)
                {
                    // add lookup entry and then add to list
                    dataBlocks.Add(block);
                }
            }
        }

        return new SiiBinaryFile(version, structureBlocks, dataBlocks);
    }

    private static SiiTextFile DecodePlainSii(byte[] data, ILogger<SiiStream>? logger = null)
    {
        var decodedString = Encoding.UTF8.GetString(data);
        var decoder = new PlainSiiDecoder(decodedString);
        return decoder.ParseFile();
    }
}