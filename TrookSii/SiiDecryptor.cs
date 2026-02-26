using System.IO.Compression;
using System.Security.Cryptography;

namespace TrookSii;

public static class SiiDecryptor
{
    // assumptions based on test file:
    // - encrypted + compressed ("ScsC" header)
    // - decompressed data is a BSII file

    private const uint ScscHeader = 0x43736353;
    private const uint BsiiHeader = 0x49495342;

    private const int HmacSize = 32;
    private const int IvSize = 16;

    private static readonly byte[] SiiKey =
    [
        0x2a, 0x5f, 0xcb, 0x17, 0x91, 0xd2, 0x2f, 0xb6, 0x02, 0x45, 0xb3, 0xd8, 0x36, 0x9e, 0xd0, 0xb2,
        0xc2, 0x73, 0x71, 0x56, 0x3f, 0xbf, 0x1f, 0x3c, 0x9e, 0xdf, 0x6b, 0x11, 0x82, 0x5a, 0x5d, 0x0a
    ];
    
    public static async Task<byte[]> DecryptScsc(byte[] scscBytes)
    {
        var idx = 0;
        
        // verify file type
        var headerSignature = BitConverter.ToUInt32(scscBytes, idx);
        if (headerSignature != ScscHeader)
        {
            throw new InvalidOperationException("Given file is not an ScsC file");
        }

        idx += sizeof(uint);
        
        // get hmac, iv, datasize
        var hmac = scscBytes[idx..(idx += HmacSize)];
        var iv = scscBytes[idx..(idx += IvSize)];
        var dataSize = BitConverter.ToUInt32(scscBytes, idx);
        idx += sizeof(uint);

        var data = scscBytes[idx..];

        using var aes = Aes.Create();
        aes.Key = SiiKey;
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor();
        var decryptedData = decryptor.TransformFinalBlock(data, 0, data.Length);

        Console.WriteLine($"nominal data size is {dataSize}");
        Console.WriteLine($"decrypted compressed data size is {decryptedData.Length}");
        
        // decompress
        var zIn = new MemoryStream(decryptedData);
        var z = new ZLibStream(zIn, CompressionMode.Decompress);
        var zOut = new MemoryStream();
        await z.CopyToAsync(zOut);
        var decompressed = zOut.ToArray();

        Console.WriteLine($"finished decompressing, final size is {decompressed.Length}");

        return decompressed;
    }
}