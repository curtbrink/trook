using System.Text;
using TrookSii.Types;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamTypeExtensions
{
    public static readonly char[] EncodedChars =
    [ // "@" is index 0 and is unused.
        '@', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
        'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
        'w', 'x', 'y', 'z', '_'
    ];

    public static string DecodeEncodedString(ulong enc)
    {
        var s = new StringBuilder("");
        while (enc > 0)
        {
            var m = enc % 38;
            s.Append(EncodedChars[m]);
            enc /= 38;
        }

        return s.ToString();
    }
    
    extension(SiiStream sii)
    {
        public string ReadString()
        {
            var strLen = (int) sii.ReadUInt32();
            var b = sii.ReadBytes(strLen);
            return Encoding.UTF8.GetString(b);
        }
        
        public string ReadEncodedString()
        {
            var enc = sii.ReadUInt64();
            return DecodeEncodedString(enc);
        }
        
        public string ReadDataBlockId()
        {
            var len = sii.ReadBytes(1).Single();

            if (len == 255)
            {
                var id = sii.ReadUInt64();
                return $"_nameless.{id:X}";
            }
            
            List<string> parts = [];
            for (var i = 0; i < len; i++)
            {
                var part = sii.ReadUInt64();
                var partStr = DecodeEncodedString(part);
                parts.Add(partStr);
            }

            return string.Join(".", parts);
        }
        
        public float[] ReadVec8S()
        {
            // by far the weirdest one so far...
            var allFloats = sii.ReadNFloat(8);

            var baseBias = (int)allFloats[3]; // fourth component is the special one

            var biasedA = baseBias & 0xFFF;
            biasedA -= 2048;
            biasedA <<= 9;
            var finalA = biasedA + allFloats[0];

            var biasedC = baseBias >> 12;
            biasedC &= 0xFFF;
            biasedC -= 2048;
            biasedC <<= 9;
            var finalC = biasedC + allFloats[2];

            return [finalA, allFloats[1], finalC, allFloats[4], allFloats[5], allFloats[6], allFloats[7]];
        }

        public float[] ReadVec2S() => sii.ReadNFloat(2);

        public float[] ReadVec3S() => sii.ReadNFloat(3);

        public float[] ReadVec4S() => sii.ReadNFloat(4);

        public int[] ReadVec3I() => sii.ReadNInt32(3);
    }
}