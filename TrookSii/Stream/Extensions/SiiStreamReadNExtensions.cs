// ReSharper disable InconsistentNaming

using TrookSii.Types;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamReadNExtensions
{
    extension(SiiStream sii)
    {
        public bool[] ReadNBool(int n)
        {
            var v = new bool[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadBool();
            }

            return v;
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

        public ushort[] ReadNUInt16(int n)
        {
            var v = new ushort[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadUInt16();
            }

            return v;
        }

        public uint[] ReadNUInt32(int n)
        {
            var v = new uint[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadUInt32();
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

        public long[] ReadNInt64(int n)
        {
            var v = new long[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadInt64();
            }

            return v;
        }
        
        public float[][] ReadNVec3S(int n)
        {
            var v = new float[n][];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadVec3S();
            }

            return v;
        }
        
        public float[][] ReadNVec4S(int n)
        {
            var v = new float[n][];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadVec4S();
            }

            return v;
        }

        public float[][] ReadNVec8S(int n)
        {
            var v = new float[n][];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadVec8S();
            }

            return v;
        }

        public int[][] ReadNVec3I(int n)
        {
            var v = new int[n][];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadVec3I();
            }

            return v;
        }

        public string[] ReadNString(int n)
        {
            var v = new string[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadString();
            }

            return v;
        }
        
        public string[] ReadNEncodedString(int n)
        {
            var v = new string[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadEncodedString();
            }

            return v;
        }

        public BlockId[] ReadNDataBlockId(int n)
        {
            var v = new BlockId[n];
            for (var i = 0; i < n; i++)
            {
                v[i] = sii.ReadDataBlockId();
            }

            return v;
        }
    }
}