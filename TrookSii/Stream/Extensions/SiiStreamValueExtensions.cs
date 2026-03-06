using Microsoft.Extensions.Logging;
using TrookSii.Types.Raw;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamValueExtensions
{
    extension(SiiStream sii)
    {
        public SiiValue GetValueForDefinition(ValueDefinition vd, StructureBlock structureBlock,
            ILogger<SiiStream>? logger = null)
        {
            logger?.LogInformation($"==> field: {vd.Name} (type = 0x{vd.TypeId:X})");
            return vd.TypeId switch
            {
                0x01 => sii.ReadStringPrimitive(),
                0x02 => sii.ReadStringArray(),
                0x03 => sii.ReadEncodedStringPrimitive(),
                0x04 => sii.ReadEncodedStringArray(),
                0x05 => sii.ReadFloatPrimitive(),
                0x06 => sii.ReadFloatArray(),
                0x07 => sii.ReadVec2S(),
                0x09 => sii.ReadVec3S(),
                0x0a => sii.ReadVec3SArray(),
                0x11 => sii.ReadVec3I(),
                0x12 => sii.ReadVec3IArray(),
                0x17 => sii.ReadVec4S(),
                0x18 => sii.ReadVec4SArray(),
                0x19 => sii.ReadVec8S(),
                0x1a => sii.ReadVec8SArray(),
                0x25 => sii.ReadInt32Primitive(),
                0x26 => sii.ReadInt32Array(),
                0x27 or 0x2f => sii.ReadUInt32Primitive(),
                0x28 => sii.ReadUInt32Array(),
                0x2b => sii.ReadUInt16Primitive(),
                0x2c => sii.ReadUInt16Array(),
                0x31 => sii.ReadInt64Primitive(),
                0x32 => sii.ReadInt64Array(),
                0x33 => sii.ReadUInt64Primitive(),
                0x34 => sii.ReadUInt64Array(),
                0x35 => sii.ReadBoolPrimitive(),
                0x36 => sii.ReadBoolArray(),
                0x37 => sii.ReadOrdinalString(structureBlock),
                0x39 or 0x3b or 0x3d => sii.ReadBlockIdPrimitive(),
                0x3a or 0x3c => sii.ReadBlockIdArray(),
                _ => throw new InvalidOperationException($"Unknown data type! 0x{vd.TypeId:X}")
            };
        }

        public SiiPrimitive ReadStringPrimitive() => new(SiiValueType.String, sii.ReadString());

        public SiiPrimitive ReadEncodedStringPrimitive() => new(SiiValueType.EncodedString, sii.ReadEncodedString());

        public SiiPrimitive ReadFloatPrimitive() => new(SiiValueType.Float, sii.ReadFloat());

        public SiiPrimitive ReadInt32Primitive() => new(SiiValueType.Int, sii.ReadInt32());

        public SiiPrimitive ReadInt64Primitive() => new(SiiValueType.Long, sii.ReadInt64());

        public SiiPrimitive ReadUInt32Primitive() => new(SiiValueType.UInt, sii.ReadUInt32());

        public SiiPrimitive ReadUInt16Primitive() => new(SiiValueType.UShort, sii.ReadUInt16());

        public SiiPrimitive ReadUInt64Primitive() => new(SiiValueType.ULong, sii.ReadUInt64());

        public SiiPrimitive ReadBoolPrimitive() => new(SiiValueType.Bool, sii.ReadBool());

        public SiiPrimitive ReadBlockIdPrimitive() => new(SiiValueType.BlockId, sii.ReadDataBlockId());
        
        public SiiVector ReadVec2S() => new(SiiValueType.Float, sii.ReadNFloat(2));

        public SiiVector ReadVec3S() => new(SiiValueType.Float, sii.ReadNFloat(3));

        public SiiVector ReadVec4S() => new(SiiValueType.Float, sii.ReadNFloat(4));

        public SiiVector ReadVec3I() => new(SiiValueType.Int, sii.ReadNInt32(3));
        
        public SiiArray ReadStringArray() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.String, sii.ReadStringPrimitive);

        public SiiArray ReadEncodedStringArray() => sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.EncodedString,
            sii.ReadEncodedStringPrimitive);

        public SiiArray ReadFloatArray() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.Float, sii.ReadFloatPrimitive);

        public SiiArray ReadInt32Array() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.Int, sii.ReadInt32Primitive);

        public SiiArray ReadInt64Array() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.Long, sii.ReadInt64Primitive);

        public SiiArray ReadUInt32Array() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.UInt, sii.ReadUInt32Primitive);

        public SiiArray ReadUInt16Array() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.UShort, sii.ReadUInt16Primitive);

        public SiiArray ReadUInt64Array() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.ULong, sii.ReadUInt64Primitive);

        public SiiArray ReadBoolArray() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.Bool, sii.ReadBoolPrimitive);

        public SiiArray ReadVec3SArray() => sii.ReadArray(SiiWrapperType.Vector, SiiValueType.Float, sii.ReadVec3S);

        public SiiArray ReadVec4SArray() => sii.ReadArray(SiiWrapperType.Vector, SiiValueType.Float, sii.ReadVec4S);

        public SiiArray ReadVec8SArray() => sii.ReadArray(SiiWrapperType.Vector, SiiValueType.Float, sii.ReadVec8S);
        
        public SiiArray ReadVec3IArray() => sii.ReadArray(SiiWrapperType.Vector, SiiValueType.Int, sii.ReadVec3I);

        public SiiArray ReadBlockIdArray() =>
            sii.ReadArray(SiiWrapperType.Primitive, SiiValueType.BlockId, sii.ReadBlockIdPrimitive);

        public SiiArray ReadArray(SiiWrapperType w, SiiValueType t, Func<SiiValue> getter)
        {
            var l = (int)sii.ReadUInt32();
            var v = new SiiValue[l];
            for (var i = 0; i < l; i++)
            {
                v[i] = getter();
            }

            return new SiiArray(w, t, v);
        }
        
        // special types

        public SiiPrimitive ReadOrdinalString(StructureBlock structure)
        {
            var idx = sii.ReadUInt32();
            var s = structure.GetOrdinalString(idx);
            if (s is null)
                throw new InvalidOperationException(
                    $"Structure block with id={structure.Id} does not contain ordinal string with index={idx}");
            return new SiiPrimitive(SiiValueType.String, s);
        }
        
        public SiiVector ReadVec8S()
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

            float[] v = [finalA, allFloats[1], finalC, allFloats[4], allFloats[5], allFloats[6], allFloats[7]];

            return new SiiVector(SiiValueType.Float, v);
        }
    }
}