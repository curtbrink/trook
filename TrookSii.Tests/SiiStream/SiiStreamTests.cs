using TrookSii.Stream;

namespace TrookSii.Tests;

public class SiiStreamTests
{
    [Fact]
    public void SiiStream_ReadBytesAndDumpBytesTests()
    {
        byte[] bytes = [0xFF, 0x01, 0xFE, 0x02];
        var s = new SiiStream(ref bytes);

        var firstTwo = s.ReadBytes(2);
        Assert.Equal(2, firstTwo.Length);
        Assert.Equal(0xFF, firstTwo[0]);
        Assert.Equal(0x01, firstTwo[1]);

        var rem = s.DumpRemainingBytes();
        Assert.Equal(2, rem.Length);
        Assert.Equal(0xFE, rem[0]);
        Assert.Equal(0x02, rem[1]);
    }

    [Fact]
    public void SiiStream_ReadBoolTests()
    {
        byte[] bytes = [0x00, 0x01, 0x00, 0x00, 0x05];
        var s = new SiiStream(ref bytes);

        var b = new bool[5];
        for (var i = 0; i < 5; i++)
        {
            b[i] = s.ReadBool();
        }

        Assert.False(b[0]);
        Assert.True(b[1]);
        Assert.False(b[2]);
        Assert.False(b[3]);
        Assert.True(b[4]);
    }

    [Fact]
    public void SiiStream_ReadFloatTests()
    {
        const float f1 = 5.678f;
        const float f2 = 763.1234f;

        byte[] bytes = [0x2D, 0xB2, 0xB5, 0x40, 0xE6, 0xC7, 0x3E, 0x44];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadFloat();
        var b2 = s.ReadFloat();
        
        Assert.Equal(f1, b1);
        Assert.Equal(f2, b2);
    }
    
    [Fact]
    public void SiiStream_ReadUInt16Tests()
    {
        const ushort us1 = 34324;
        const ushort us2 = 9812;

        byte[] bytes = [0x14, 0x86, 0x54, 0x26];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadUInt16();
        var b2 = s.ReadUInt16();
        
        Assert.Equal(us1, b1);
        Assert.Equal(us2, b2);
    }
    
    [Fact]
    public void SiiStream_ReadUInt32Tests()
    {
        const uint ui1 = 957296445;
        const uint ui2 = 2144972974;

        byte[] bytes = [0x3D, 0x2F, 0x0F, 0x39, 0xAE, 0xB0, 0xD9, 0x7F];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadUInt32();
        var b2 = s.ReadUInt32();
        
        Assert.Equal(ui1, b1);
        Assert.Equal(ui2, b2);
    }
    
    [Fact]
    public void SiiStream_ReadUInt64Tests()
    {
        const ulong ul1 = 8032667056067599723;
        const ulong ul2 = 2702121465127287093;

        byte[] bytes =
        [
            0x6B, 0x5D, 0xE4, 0xEB, 0x20, 0xC4, 0x79, 0x6F,
            0x35, 0xB9, 0xDE, 0xF4, 0x27, 0xDD, 0x7F, 0x25
        ];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadUInt64();
        var b2 = s.ReadUInt64();
        
        Assert.Equal(ul1, b1);
        Assert.Equal(ul2, b2);
    }
    
    [Fact]
    public void SiiStream_ReadInt32Tests()
    {
        const int i1 = 820595558;
        const int i2 = -1937059162;

        byte[] bytes = [0x66, 0x4B, 0xE9, 0x30, 0xA6, 0xD2, 0x8A, 0x8C];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadInt32();
        var b2 = s.ReadInt32();
        
        Assert.Equal(i1, b1);
        Assert.Equal(i2, b2);
    }
    
    [Fact]
    public void SiiStream_ReadInt64Tests()
    {
        const long l1 = -3094998070388668113;
        const long l2 = 8328472983518533342;

        byte[] bytes =
        [
            0x2F, 0xC9, 0xF6, 0x29, 0xA7, 0x5B, 0x0C, 0xD5,
            0xDE, 0x56, 0x0F, 0x68, 0x0D, 0xAE, 0x94, 0x73
        ];
        var s = new SiiStream(ref bytes);

        var b1 = s.ReadInt64();
        var b2 = s.ReadInt64();
        
        Assert.Equal(l1, b1);
        Assert.Equal(l2, b2);
    }
}