using System.Text;
using TrookSii.Types;

namespace TrookSii;

public class SiiStream(ref byte[] buffer)
{
    private readonly byte[] _buffer = buffer;
    
    private int _idx = 0;

    public byte[] ReadBytes(int count)
    {
        var v = _buffer[_idx..(_idx += count)];
        return v;
    }
    
    public uint ReadUInt32(bool peek = false)
    {
        var v = BitConverter.ToUInt32(_buffer, _idx);
        if (!peek) _idx += 4;
        return v;
    }

    public ulong ReadUInt64()
    {
        var v = BitConverter.ToUInt64(_buffer, _idx);
        _idx += 8;
        return v;
    }

    public bool ReadBoolByte()
    {
        var v = BitConverter.ToBoolean(_buffer, _idx);
        _idx++;
        return v;
    }

    public string ReadString()
    {
        var strLen = (int) ReadUInt32();
        var b = ReadBytes(strLen);
        return Encoding.UTF8.GetString(b);
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

    public byte[] DumpRemainingBytes()
    {
        var v = _buffer[_idx..];
        _idx = _buffer.Length;
        return v;
    }
}