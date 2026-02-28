using TrookSii.Stream;
using TrookSii.Stream.Extensions;

namespace TrookSii.Tests;

public class SiiStreamArrayTests
{
    [Fact]
    public void SiiStream_ReadBoolArrayTests()
    {
        bool[] bs = [true, true, false, true, false];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadBoolArray();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadFloatArrayTests()
    {
        float[] bs = [1.234f, 0.1f, 876f, 34.56f, 0.00001f];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadFloatArray();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadUInt16ArrayTests()
    {
        ushort[] bs = [2, 45, 1, 32, 200];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadUInt16Array();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadUInt32ArrayTests()
    {
        uint[] bs = [2, 45, 1, 32, 200];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadUInt32Array();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadUInt64ArrayTests()
    {
        ulong[] bs = [2, 45, 1, 32, 200];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadUInt64Array();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadInt32ArrayTests()
    {
        int[] bs = [2, 45, 1, 32, 200];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadInt32Array();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    [Fact]
    public void SiiStream_ReadInt64ArrayTests()
    {
        long[] bs = [2, 45, 1, 32, 200];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadInt64Array();
        Assert.Equal(5, bArray.Length);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(bs[i], bArray[i]);
        }
    }
    
    // vector types
    
    [Fact]
    public void SiiStream_ReadVec3IArrayTests()
    {
        int[][] bs = [[2, 45, 1], [32, 200, 0]];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadVec3IArray();
        Assert.Equal(2, bArray.Length);
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Assert.Equal(bs[i][j], bArray[i][j]);
            }
        }
    }
    
    [Fact]
    public void SiiStream_ReadVec3SArrayTests()
    {
        float[][] bs = [[2f, 45f, 200f], [32f, 200f, 0f]];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadVec3SArray();
        Assert.Equal(2, bArray.Length);
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                Assert.Equal(bs[i][j], bArray[i][j]);
            }
        }
    }
    
    [Fact]
    public void SiiStream_ReadVec4SArrayTests()
    {
        float[][] bs = [[2f, 45f, 200f, 657f], [32f, 200f, 0f, 12345f]];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadVec4SArray();
        Assert.Equal(2, bArray.Length);
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                Assert.Equal(bs[i][j], bArray[i][j]);
            }
        }
    }
    
    [Fact]
    public void SiiStream_ReadVec8SArrayTests()
    {
        var (f1A, f1B) = SiiStreamTypeTests.GetSampleVec8S();
        var (f2A, f2B) = SiiStreamTypeTests.GetSampleVec8S();
        float[][] bs = [f1A, f2A];
        var bytes = TypeHelpers.EncodeArray(bs);

        var s = new SiiStream(ref bytes);

        var bArray = s.ReadVec8SArray();
        Assert.Equal(2, bArray.Length);
        for (var i = 0; i < 7; i++)
        {
            Assert.Equal(f1B[i], bArray[0][i]);
        }
        for (var i = 0; i < 7; i++)
        {
            Assert.Equal(f2B[i], bArray[1][i]);
        }
    }
}