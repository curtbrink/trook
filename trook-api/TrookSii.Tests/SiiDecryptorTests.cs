namespace TrookSii.Tests;

public class SiiDecryptorTests
{
    [Theory]
    [InlineData("testsave.sii")]
    [InlineData("testsave_withjobs.sii")]
    public async Task SiiDecryptor_DecryptsValidFile(string fileName)
    {
        var bytes = await File.ReadAllBytesAsync(fileName);
        var processed = await SiiDecryptor.DecryptScsc(bytes);

        const uint expectedHeader = 0x49495342;
        var dataHeader = BitConverter.ToUInt32(processed);
        Assert.Equal(expectedHeader, dataHeader);
    }

    [Fact]
    public async Task SiiDecryptor_ThrowsForInvalidFiles()
    {
        var r = new Random();
        var b = new byte[100];
        r.NextBytes(b);

        await Assert.ThrowsAnyAsync<Exception>(() => SiiDecryptor.DecryptScsc(b));
    }
}