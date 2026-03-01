using System.Text;
using TrookSii.Types.Raw;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamTypeExtensions
{
    extension(SiiStream sii)
    {
        public string ReadString()
        {
            var strLen = (int) sii.ReadUInt32();
            var b = sii.ReadBytes(strLen);
            return Encoding.UTF8.GetString(b);
        }
        
        public EncodedString ReadEncodedString()
        {
            var enc = sii.ReadUInt64();
            return new EncodedString(enc);
        }
        
        public BlockId ReadDataBlockId()
        {
            var len = sii.ReadBytes(1).Single();
            
            var partsToRead = len == 255 ? 1 : len;
            var parts = sii.ReadNUInt64(partsToRead);

            return new BlockId(len, parts);
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