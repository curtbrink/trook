using Microsoft.Extensions.Logging;
using TrookSii.Types.Raw;

namespace TrookSii.Stream.Extensions;

public static class SiiStreamValueExtensions
{
    public static dynamic GetValueForDefinition(this SiiStream sii, ValueDefinition vd, StructureBlock structureBlock,
        ILogger<SiiStream>? logger = null)
    {
        dynamic vdValue;
        logger?.LogInformation($"==> field: {vd.Name} (type = 0x{vd.TypeId:X})");
        switch (vd.TypeId)
        {
            case 0x01:
                vdValue = sii.ReadString();
                logger?.LogInformation($"====> string: {vdValue}");
                break;
            case 0x02:
                vdValue = sii.ReadStringArray();
                foreach (var s in vdValue)
                    logger?.LogInformation($"====> string: {s}");
                break;
            case 0x03:
                vdValue = sii.ReadEncodedString();
                logger?.LogInformation($"====> enc string: {vdValue}");
                break;
            case 0x04:
                vdValue = sii.ReadEncodedStringArray();
                foreach (var s in vdValue)
                    logger?.LogInformation($"====> enc string: {s}");
                break;
            case 0x05:
                vdValue = sii.ReadFloat();
                logger?.LogInformation($"====> float: {vdValue}");
                break;
            case 0x06:
                vdValue = sii.ReadFloatArray();
                foreach (var sf in vdValue)
                    logger?.LogInformation($"====> float: {sf}");
                break;
            case 0x07:
                vdValue = sii.ReadVec2S();
                logger?.LogInformation($"====> vec2floats: [{vdValue[0]}, {vdValue[1]}]");
                break;
            case 0x09:
                vdValue = sii.ReadVec3S();
                logger?.LogInformation($"====> vec3floats: [{vdValue[0]}, {vdValue[1]}, {vdValue[2]}]");
                break;
            case 0x11:
                vdValue = sii.ReadVec3I();
                foreach (var v in vdValue)
                    logger?.LogInformation($"====> int: {v}");
                break;
            case 0x12:
                vdValue = sii.ReadVec3IArray();
                foreach (var vec3 in vdValue)
                    logger?.LogInformation($"====> vec3: [{vec3[0]}, {vec3[1]}, {vec3[2]}]");
                break;
            case 0x18:
                vdValue = sii.ReadVec4SArray();
                foreach (var vec4A in vdValue)
                {
                    logger?.LogInformation("====> vec4s array:");
                    foreach (var vec4 in vec4A)
                        logger?.LogInformation($"======> float: {vec4}");
                }

                break;
            case 0x19:
                vdValue = sii.ReadVec8S();
                foreach (var weirdFloat in vdValue)
                    logger?.LogInformation($"====> biased float: {weirdFloat}");
                break;
            case 0x1a:
                vdValue = sii.ReadVec8SArray();
                foreach (var wfa in vdValue)
                {
                    logger?.LogInformation($"====> weird float array:");
                    foreach (var wfa2 in wfa)
                        logger?.LogInformation($"======> weird float: {wfa2}");
                }

                break;
            case 0x25:
                vdValue = sii.ReadInt32();
                logger?.LogInformation($"====> int: {vdValue}");
                break;
            case 0x26:
                vdValue = sii.ReadInt32Array();
                foreach (var s in vdValue)
                    logger?.LogInformation($"====> int: {s}");
                break;
            case 0x27:
                vdValue = sii.ReadUInt32();
                logger?.LogInformation($"====> uint: {vdValue}");
                break;
            case 0x28:
                vdValue = sii.ReadUInt32Array();
                foreach (var nsv in vdValue)
                    logger?.LogInformation($"====> uint: {vdValue}");
                break;
            case 0x2b:
                vdValue = sii.ReadUInt16();
                logger?.LogInformation($"====> ushort: {vdValue}");
                break;
            case 0x2c:
                vdValue = sii.ReadUInt16Array();
                foreach (var us in vdValue)
                    logger?.LogInformation($"====> ushort: {us}");
                break;
            case 0x2f:
                vdValue = sii.ReadUInt32();
                logger?.LogInformation($"====> uint: {vdValue}");
                break;
            case 0x31:
                vdValue = sii.ReadInt64();
                logger?.LogInformation($"====> long: {vdValue}");
                break;
            case 0x32:
                vdValue = sii.ReadInt64Array();
                foreach (var slv in vdValue)
                    logger?.LogInformation($"====> long: {slv}");
                break;
            case 0x33:
                vdValue = sii.ReadUInt64();
                logger?.LogInformation($"====> ulong: {vdValue}");
                break;
            case 0x34:
                vdValue = sii.ReadUInt64Array();
                foreach (var ui64 in vdValue)
                    logger?.LogInformation($"====> ulong: {ui64}");
                break;
            case 0x35:
                vdValue = sii.ReadBool();
                logger?.LogInformation($"====> bool: {vdValue}");
                break;
            case 0x36:
                vdValue = sii.ReadBoolArray();
                foreach (var b in vdValue)
                    logger?.LogInformation($"====> bool: {b}");
                break;
            case 0x37:
                // ordinal strings on the comeback
                var ordIdx = sii.ReadUInt32();
                vdValue = structureBlock.GetOrdinalString(ordIdx) ?? "";
                logger?.LogInformation($"====> ordinal string: {vdValue}");
                break;
            case 0x39:
            case 0x3b:
            case 0x3d:
                vdValue = sii.ReadDataBlockId();
                logger?.LogInformation($"====> block id: {vdValue}");
                break;
            case 0x3a:
            case 0x3c:
                vdValue = sii.ReadDataBlockIdArray();
                foreach (var b in vdValue)
                    logger?.LogInformation($"====> block list: {b}");
                break;
            default:
                throw new InvalidOperationException($"Unknown data type! 0x{vd.TypeId:X}");
        }

        return vdValue;
    }
}