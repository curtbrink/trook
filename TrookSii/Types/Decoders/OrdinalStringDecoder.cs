using Microsoft.Extensions.Logging;
using TrookSii.Stream;
using TrookSii.Stream.Extensions;

namespace TrookSii.Types.Decoders;

public static class OrdinalStringDecoder
{
    public static IDictionary<uint, string> Decode(SiiStream sii, string name,
        ILogger<SiiStream>? logger = null)
    {
        var numStrings = sii.ReadUInt32();
        var ordinalDict = new Dictionary<uint, string>();
        for (uint i = 0; i < numStrings; i++)
        {
            var ord = sii.ReadUInt32();
            var s = sii.ReadString();
            logger?.LogInformation($"{name}[{ord}]: {s}");
            ordinalDict[ord] = s;
        }

        return ordinalDict;
    }
}