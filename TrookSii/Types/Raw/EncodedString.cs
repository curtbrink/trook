using System.Text;

namespace TrookSii.Types.Raw;

public readonly record struct EncodedString
{
    private readonly ulong _raw;
    private readonly string _decodedString;
    
    public EncodedString(ulong raw)
    {
        _raw = raw;
        _decodedString = DecodeEncodedString(raw);
    }
    
    public override string ToString() => _decodedString;

    public bool Equals(EncodedString other) => _raw == other._raw;

    public override int GetHashCode() => _raw.GetHashCode();
    
    private static readonly char[] CharMap =
    [ // "@" is index 0 and is unused.
        '@', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
        'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
        'w', 'x', 'y', 'z', '_'
    ];

    public static ReadOnlySpan<char> CharacterMap => new(CharMap);
    
    private static string DecodeEncodedString(ulong enc)
    {
        var s = new StringBuilder("");
        while (enc > 0)
        {
            var m = enc % 38;
            if (m == 0)
                throw new ArgumentOutOfRangeException(nameof(enc),
                    "invalid char index found when decoding ulong - is this really an encoded string?");
            s.Append(CharMap[m]);
            enc /= 38;
        }

        var output = s.ToString();
        if (output.Contains('@'))
            throw new ArgumentOutOfRangeException(nameof(enc),
                "this should have been caught already but output contains a '@' - invalid encoded string");

        return output;
    }
}