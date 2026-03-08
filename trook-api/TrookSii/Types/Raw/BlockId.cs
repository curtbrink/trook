namespace TrookSii.Types.Raw;

public readonly record struct BlockId
{
    private readonly byte _length;
    private readonly ulong[] _parts;

    public string Key { get; init; }

    public bool IsEmpty => _length == 0;

    public BlockId(byte length, ulong[] parts)
    {
        // verify args
        if (length == 0xFF && parts.Length != 1)
            throw new ArgumentOutOfRangeException(nameof(parts),
                $"length byte is 0xFF; expected exactly one id part but got {parts.Length}");
        if (length != 0xFF && parts.Length != length)
            throw new ArgumentOutOfRangeException(nameof(parts),
                $"expected l={length} id parts but got {parts.Length}");
        
        _length = length;
        _parts = parts;

        Key = BuildKey(length, parts);
    }

    public BlockId(string stringId)
    {
        // assuming it's a _nameless.*
        _length = 0xFF;
        var partsString = string.Join("", stringId[10..].Split('.'));
        if (partsString.Length % 2 != 0) partsString = $"0{partsString}";

        var bytes = new byte[8];
        Convert.FromHexString(partsString, bytes, out _, out _);
        _parts = [BitConverter.ToUInt64(bytes, 0)];

        Key = BuildKey(_length, _parts);
    }

    private static string BuildKey(byte l, ulong[] p)
    {
        // nameless id is special
        if (l == 0xFF)
        {
            return $"_nameless.{p[0]:X}";
        }
        
        // otherwise it's a list of encoded strings
        return string.Join(".", p.Select(part => new EncodedString(part)));
    }
}