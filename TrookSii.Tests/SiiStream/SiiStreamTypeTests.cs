using System.Text;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;

namespace TrookSii.Tests;

public class SiiStreamTypeTests
{
    [Theory]
    [InlineData("foostr", true)]
    [InlineData("itsastring", true)]
    [InlineData("under_score", true)]
    [InlineData("a", true)]
    [InlineData("", true)]
    [InlineData("this_string_is_too_long_and_would_overflow_an_ulong", false)]
    public void SiiStream_EncodedStringTests(string testStr, bool isValid)
    {
        var encoded = TypeHelpers.EncodeString(testStr);
        var bytes = BitConverter.GetBytes(encoded);

        var s = new SiiStream(ref bytes);

        var decoded = s.ReadEncodedString();

        if (isValid)
            Assert.Equal(testStr, decoded);
        else
            Assert.NotEqual(testStr, decoded);
    }
    
    [Theory]
    [InlineData("foostr")]
    [InlineData("itsastring")]
    [InlineData("under_score")]
    [InlineData("a")]
    [InlineData("")]
    [InlineData("this_string_is_too_long_and_would_overflow_an_ulong")]
    public void SiiStream_StringTests(string testStr)
    {
        var encoded = TypeHelpers.EncodeUtf8String(testStr);

        var s = new SiiStream(ref encoded);

        var decoded = s.ReadString();

        Assert.Equal(testStr, decoded);
    }

    [Fact]
    public void SiiStream_DataBlockId_NamelessTests()
    {
        const byte len = 0xFF;
        const ulong namePart = 0x123456789ABCDEF0;
        var nameBytes = BitConverter.GetBytes(namePart);
        byte[] idBytes = [len, ..nameBytes];

        var s = new SiiStream(ref idBytes);

        var id = s.ReadDataBlockId();

        Assert.Equal("_nameless.123456789ABCDEF0", id);
    }
    
    [Fact]
    public void SiiStream_DataBlockId_NamedTests()
    {
        // build byte array for block id
        string[] nameParts = ["my", "name", "is", "foobarbaz"];
        var encodedNameParts = nameParts.Select(TypeHelpers.EncodeString).ToArray();
        const byte len = 0x04;

        List<byte> blockIdBytes = [len];
        foreach (var part in encodedNameParts)
        {
            blockIdBytes.AddRange(BitConverter.GetBytes(part));
        }

        var b = blockIdBytes.ToArray();
        var s = new SiiStream(ref b);

        var id = s.ReadDataBlockId();

        Assert.Equal("my.name.is.foobarbaz", id);
    }

    [Fact]
    public void SiiStream_Vec8STests() // included here because of its complexity
    {
        var (encodedFloats, expectedFloats) = GetSampleVec8S();
        
        List<byte> fBytes = [];
        foreach (var f in encodedFloats)
        {
            fBytes.AddRange(BitConverter.GetBytes(f));
        }

        var b = fBytes.ToArray();
        var s = new SiiStream(ref b);

        var vec8S = s.ReadVec8S();

        Assert.Equal(7, vec8S.Length);
        for (var i = 0; i < 7; i++)
        {
            Assert.Equal(expectedFloats[i], vec8S[i]);
        }
    }

    public static (float[], float[]) GetSampleVec8S()
    {
        // helper method for grabbing a test vec8s type because encoding a test one is hard
        // returns (encodedFloats, decodedFloats)
        float[] encodedFloats =
        [
            76.905571f, 40.2594757f, -271.742096f, 8304703f, 0.96878463f, 0.000482589501f, -0.247903064f,
            0.000467125035f
        ];
        float[] expectedFloats =
            [32332.9062f, 40.2594757f, -11023.7422f, 0.96878463f, 0.000482589501f, -0.247903064f, 0.000467125035f];

        return (encodedFloats, expectedFloats);
    }
}