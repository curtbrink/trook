using System.Reflection;
using TrookApi.Domain;
using TrookSii.Types;

namespace TrookApi.Mappings;

public static class DataBlockExtensions
{
    extension(DataBlock dataBlock)
    {
        public ProfitLogEntry ToProfitLogEntry()
        {
            if (dataBlock.StructureId != ProfitLogEntry.StructId)
                throw new InvalidOperationException("Data block is not a ProfitLogEntry");

            var returnVal = new ProfitLogEntry();
            returnVal.MapSiiValuesFromBlock(dataBlock);
            return returnVal;
        }
    }

    private static void MapSiiValuesFromBlock<T>(this T result, DataBlock dataBlock)
    {
        var propInfos = typeof(T).GetProperties();
        foreach (var pi in propInfos)
        {
            var vdName = pi.GetCustomAttribute<SiiAttribute>()?.SiiName;
            if (vdName is null) continue;

            try
            {
                var vd = dataBlock.Data.First(t => t.Item1.Name == vdName && t.Item2.GetType() == pi.PropertyType);
                pi.SetValue(result, Convert.ChangeType(vd.Item2, pi.PropertyType));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Data type mismatch for property '{pi.Name}'", ex);
            }
        }
    }
}