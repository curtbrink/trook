using System.Reflection;
using TrookSii.Types.Models;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Mappings;

public static class DataBlockExtensions
{
    extension(DataBlock dataBlock)
    {
        public ProfitLog ToProfitLog()
        {
            dataBlock.CheckStructId(14, "ProfitLog");
            var returnVal = new ProfitLog(dataBlock.BlockId);
            returnVal.MapSiiValuesFromBlock(dataBlock);
            return returnVal;
        }
        
        public ProfitLogEntry ToProfitLogEntry()
        {
            dataBlock.CheckStructId(15, "ProfitLogEntry");
            var returnVal = new ProfitLogEntry(dataBlock.BlockId);
            returnVal.MapSiiValuesFromBlock(dataBlock);
            return returnVal;
        }
    }

    private static void CheckStructId(this DataBlock dataBlock, uint id, string modelType)
    {
        if (dataBlock.StructureId != id)
            throw new InvalidOperationException(
                $"Data block is not a {modelType} - expected structure id {id} but got {dataBlock.StructureId}");
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