namespace TrookSii.Tests;

public class SiiDecoderTests
{
    [Theory]
    [InlineData("testsave.sii", 43, 26176)]
    [InlineData("testsave_withjobs.sii", 44, 22981)]
    public async Task SiiDecoder_DecodesValidFileTests(string f, int structCount, int dataCount)
    {
        var decryptedData = await GetDecodedData(f);

        var sii = SiiDecoder.DecodeSii(decryptedData);

        Assert.Equal(structCount, sii.Structures.Count);
        Assert.Equal(dataCount, sii.Data.Count);
    }
    
    private static async Task<byte[]> GetDecodedData(string f)
    {
        var b = await File.ReadAllBytesAsync(f);
        return await SiiDecryptor.DecryptScsc(b);
    }
}