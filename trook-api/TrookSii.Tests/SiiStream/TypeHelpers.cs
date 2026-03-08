using System.Text;
using TrookSii.Types.Raw;

namespace TrookSii.Tests.SiiStream;

// util records for encoding arrays. not used directly
record EncString(string S);

public static class TypeHelpers
{
    public static ulong EncodeString(string s)
    {
        ulong encoded = 0L;

        for (var i = s.Length - 1; i >= 0; i--)
        {
            var cIdx = EncodedString.CharacterMap.IndexOf(s[i]);
            if (cIdx is < 1 or > 37)
                throw new ArgumentOutOfRangeException(nameof(s), $"Invalid char '{s[i]}' in string");
            encoded *= 38L;
            encoded += (ulong) cIdx;
        }

        return encoded;
    }

    public static byte[] EncodeUtf8String(string s)
    {
        var l = (uint)s.Length;
        var lBytes = BitConverter.GetBytes(l);

        var sBytes = Encoding.UTF8.GetBytes(s);
        return [..lBytes, ..sBytes];
    }

    public static byte[] EncodeEncodedStringArray(string[] s)
    {
        var encs = s.Select(c => new EncString(c)).ToArray();
        return EncodeArray(encs);
    }
    
    public static byte[] EncodeArray<T>(T[] collection)
    {
        var l = (uint)collection.Length;
        var b = new List<byte>();
        b.AddRange(BitConverter.GetBytes(l));

        foreach (var item in collection)
        {
            b.AddRange(EncodePrimitive(item));
        }

        return b.ToArray();
    }

    private static byte[] EncodePrimitive<T>(T item)
    {
        return item switch
        {
            bool b => BitConverter.GetBytes(b),
            float f => BitConverter.GetBytes(f),
            ushort us => BitConverter.GetBytes(us),
            uint ui => BitConverter.GetBytes(ui),
            ulong ul => BitConverter.GetBytes(ul),
            short sh => BitConverter.GetBytes(sh),
            int i => BitConverter.GetBytes(i),
            long l => BitConverter.GetBytes(l),
            EncString es => BitConverter.GetBytes(EncodeString(es.S)),
            string s => Encoding.UTF8.GetBytes(s),
            float[] fa => fa.SelectMany(BitConverter.GetBytes).ToArray(),
            int[] ia => ia.SelectMany(BitConverter.GetBytes).ToArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(item), "Invalid type")
        };
    }

    public static T[] UnwrapArrayPrimitives<T>(this SiiArray siia)
    {
        var values = (SiiValue[])siia.Values;
        var ts = new T[values.Length];
        for (var i = 0; i < values.Length; i++)
        {
            var p = (SiiPrimitive)values[i];
            ts[i] = (T)p.Value;
        }

        return ts;
    }

    public static T[][] UnwrapVectorPrimitives<T>(this SiiArray siia)
    {
        var values = (SiiValue[])siia.Values;
        var ts = new T[values.Length][];
        for (var i = 0; i < values.Length; i++)
        {
            var v = (SiiVector)values[i];
            ts[i] = (T[])v.Values;
        }

        return ts;
    }
}