using TrookSii.Types;
using TrookSii.Types.Raw;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamArrayExtensions
{
    extension(SiiStream sii)
    {
        public bool[] ReadBoolArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNBool(len);
        }
        
        public float[] ReadFloatArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNFloat(len);
        }

        public ushort[] ReadUInt16Array()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNUInt16(len);
        }

        public uint[] ReadUInt32Array()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNUInt32(len);
        }
        
        public ulong[] ReadUInt64Array()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNUInt64(len);
        }

        public int[] ReadInt32Array()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNInt32(len);
        }
        
        public long[] ReadInt64Array()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNInt64(len);
        }
        
        public int[][] ReadVec3IArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNVec3I(len);
        }
        
        public float[][] ReadVec3SArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNVec3S(len);
        }
        
        public float[][] ReadVec4SArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNVec4S(len);
        }
        
        public float[][] ReadVec8SArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNVec8S(len);
        }

        public string[] ReadStringArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNString(len);
        }

        public EncodedString[] ReadEncodedStringArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNEncodedString(len);
        }

        public BlockId[] ReadDataBlockIdArray()
        {
            var len = (int)sii.ReadUInt32();
            return sii.ReadNDataBlockId(len);
        }
    }
}