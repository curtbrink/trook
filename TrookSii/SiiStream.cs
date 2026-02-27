using System.Text;
using TrookSii.Types;

namespace TrookSii;

public class SiiStream(ref byte[] buffer)
{
    private static char[] _encodedChars =
    [ // "@" is index 0 and is unused.
        '@', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
        'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
        'w', 'x', 'y', 'z', '_'
    ];
    
    private readonly byte[] _buffer = buffer;
    
    private int _idx = 0;

    public byte[] ReadBytes(int count)
    {
        var v = _buffer[_idx..(_idx += count)];
        return v;
    }

    public float ReadFloat()
    {
        var v = BitConverter.ToSingle(_buffer, _idx);
        _idx += 4;
        return v;
    }

    public float[] ReadFloats(int n)
    {
        var v = new float[n];
        for (var i = 0; i < n; i++)
        {
            v[i] = ReadFloat();
        }

        return v;
    }

    public float[] ReadFloatArray()
    {
        var len = (int)ReadUInt32();
        return ReadFloats(len);
    }

    public float[] ReadBiasedFloats()
    {
        // by far the weirdest one so far...
        var allFloats = new float[8];
        for (var i = 0; i < 8; i++)
        {
            allFloats[i] = ReadFloat();
        }

        var baseBias = (int)allFloats[3]; // fourth component is the special one

        var biasedA = baseBias & 0xFFF;
        biasedA -= 2048;
        biasedA = biasedA << 9;
        var finalA = biasedA + allFloats[0];

        var biasedC = baseBias >> 12;
        biasedC &= 0xFFF;
        biasedC -= 2048;
        biasedC = biasedC << 9;
        var finalC = biasedC + allFloats[2];

        return [finalA, allFloats[1], finalC, allFloats[4], allFloats[5], allFloats[6], allFloats[7]];
    }

    public float[][] ReadBiasedFloatsArray()
    {
        var len = ReadUInt32();
        var v = new float[len][];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadBiasedFloats();
        }

        return v;
    }

    public ushort ReadUInt16()
    {
        var v = BitConverter.ToUInt16(_buffer, _idx);
        _idx += 2;
        return v;
    }

    public ushort[] ReadUInt16Array()
    {
        var len = ReadUInt32();
        var v = new ushort[len];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadUInt16();
        }

        return v;
    }

    public int ReadInt32()
    {
        var v = BitConverter.ToInt32(_buffer, _idx);
        _idx += 4;
        return v;
    }

    public int[] ReadInt32s(int n)
    {
        var v = new int[n];
        for (var i = 0; i < n; i++)
        {
            v[i] = ReadInt32();
        }

        return v;
    }

    public int[] ReadInt32Array()
    {
        var len = (int)ReadUInt32();
        return ReadInt32s(len);
    }

    public int[][] ReadVec3IArray()
    {
        var n = ReadUInt32();
        var tuples = new int[n][];
        for (var i = 0; i < n; i++)
        {
            tuples[i] = ReadInt32s(3);
        }

        return tuples;
    }

    public float[][] ReadVec4SArray()
    {
        var n = ReadUInt32();
        var tuples = new float[n][];
        for (var i = 0; i < n; i++)
        {
            tuples[i] = ReadFloats(4);
        }

        return tuples;
    }
    
    public uint ReadUInt32(bool peek = false)
    {
        var v = BitConverter.ToUInt32(_buffer, _idx);
        if (!peek) _idx += 4;
        return v;
    }

    public uint[] ReadUInt32Array()
    {
        var len = ReadUInt32();
        var v = new uint[len];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadUInt32();
        }

        return v;
    }

    public long ReadInt64()
    {
        var v = BitConverter.ToInt64(_buffer, _idx);
        _idx += 8;
        return v;
    }

    public long[] ReadInt64Array()
    {
        var len = ReadUInt32();
        var v = new long[len];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadInt64();
        }

        return v;
    }

    public ulong ReadUInt64()
    {
        var v = BitConverter.ToUInt64(_buffer, _idx);
        _idx += 8;
        return v;
    }
    
    public ulong[] ReadUInt64Array()
    {
        var len = ReadUInt32();
        var v = new ulong[len];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadUInt64();
        }

        return v;
    }

    public bool ReadBoolByte()
    {
        var v = BitConverter.ToBoolean(_buffer, _idx);
        _idx++;
        return v;
    }

    public bool[] ReadBoolByteArray()
    {
        var len = ReadUInt32();
        var v = new bool[len];
        for (var i = 0; i < len; i++)
        {
            v[i] = ReadBoolByte();
        }

        return v;
    }

    public string ReadString()
    {
        var strLen = (int) ReadUInt32();
        var b = ReadBytes(strLen);
        return Encoding.UTF8.GetString(b);
    }

    public string[] ReadStringArray()
    {
        var strs = (int)ReadUInt32();
        var s = new List<string>();
        for (var i = 0; i < strs; i++)
        {
            s.Add(ReadString());
        }

        return s.ToArray();
    }

    public string ReadEncodedString()
    {
        var enc = ReadUInt64();
        var s = new StringBuilder("");
        while (enc > 0)
        {
            var m = enc % 38;
            s.Append(_encodedChars[m]);
            enc /= 38;
        }

        return s.ToString();
    }

    public string[] ReadEncodedStringArray()
    {
        var strs = (int)ReadUInt32();
        var s = new List<string>();
        for (var i = 0; i < strs; i++)
        {
            s.Add(ReadEncodedString());
        }

        return s.ToArray();
    }

    public BlockId ReadDataBlockId()
    {
        var len = ReadBytes(1).Single();
        var isNameless = len == 255;
        var partsToRead = isNameless ? 1 : len;
        List<ulong> parts = [];
        for (var i = 0; i < partsToRead; i++)
        {
            parts.Add(ReadUInt64());
        }

        return new BlockId { Length = len, Parts = parts };
    }

    public BlockId[] ReadDataBlockIdArray()
    {
        var b = new List<BlockId>();
        var len = ReadUInt32();
        for (var i = 0; i < len; i++)
        {
            var bId = ReadDataBlockId();
            b.Add(bId);
        }

        return b.ToArray();
    }

    public byte[] DumpRemainingBytes()
    {
        var v = _buffer[_idx..];
        _idx = _buffer.Length;
        return v;
    }
}