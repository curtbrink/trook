
namespace TrookSii.Stream;

public class SiiStream(ref byte[] buffer)
{
    private readonly byte[] _buffer = buffer;
    
    private int _idx = 0;

    public byte[] ReadBytes(int count)
    {
        var v = _buffer[_idx..(_idx += count)];
        return v;
    }
    
    public bool ReadBool()
    {
        var v = BitConverter.ToBoolean(_buffer, _idx);
        _idx++;
        return v;
    }

    public float ReadFloat()
    {
        var v = BitConverter.ToSingle(_buffer, _idx);
        _idx += 4;
        return v;
    }
    
    public ushort ReadUInt16()
    {
        var v = BitConverter.ToUInt16(_buffer, _idx);
        _idx += 2;
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
    
    public int ReadInt32()
    {
        var v = BitConverter.ToInt32(_buffer, _idx);
        _idx += 4;
        return v;
    }
    
    public long ReadInt64()
    {
        var v = BitConverter.ToInt64(_buffer, _idx);
        _idx += 8;
        return v;
    }
    
    public byte[] DumpRemainingBytes()
    {
        var v = _buffer[_idx..];
        _idx = _buffer.Length;
        return v;
    }

}