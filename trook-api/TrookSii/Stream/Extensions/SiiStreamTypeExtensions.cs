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
        
        public float[] ReadNFloat(int n)
        {
            var v = new float[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadFloat();
            }

            return v;
        }

        public ulong[] ReadNUInt64(int n)
        {
            var v = new ulong[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadUInt64();
            }

            return v;
        }

        public int[] ReadNInt32(int n)
        {
            var v = new int[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadInt32();
            }

            return v;
        }
    }
}