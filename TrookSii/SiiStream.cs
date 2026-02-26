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

    public bool ReadBoolByte()
    {
        var v = BitConverter.ToBoolean(_buffer, _idx);
        _idx++;
        return v;
    }

    public byte[] DumpRemainingBytes()
    {
        var v = _buffer[_idx..];
        _idx = _buffer.Length;
        return v;
    }
}