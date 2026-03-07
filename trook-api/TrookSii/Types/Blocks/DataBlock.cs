using TrookSii.Types.Raw;

namespace TrookSii.Types.Blocks;

public class DataBlock(BlockId blockId, StructureBlock structure, IList<(ValueDefinition, SiiValue)> data)
{
    public BlockId Id { get; } = blockId;

    public StructureBlock Structure { get; } = structure;

    private readonly Dictionary<string, SiiValue> _data = data.ToDictionary(t => t.Item1.Name, t => t.Item2);

    public T GetValue<T>(string name)
    {
        var found = _data.TryGetValue(name, out var siiValue);
        if (!found) throw new KeyNotFoundException($"Key '{name}' not found in this block");

        if (siiValue is not SiiPrimitive siiPrimitive)
            throw new InvalidOperationException($"Value '{name}' is not a primitive type");

        try
        {
            return UnwrapPrimitive<T>(siiPrimitive);
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException($"Failed to unwrap primitive '{name}'", e);
        }
    }

    private T UnwrapPrimitive<T>(SiiPrimitive siiPrimitive)
    {
        return siiPrimitive.ValueType switch
        {
            SiiValueType.String when typeof(T) == typeof(string) => (T)siiPrimitive.Value,
            SiiValueType.EncodedString when typeof(T) == typeof(string) => (T)siiPrimitive.Value,
            SiiValueType.Byte when typeof(T) == typeof(byte) => (T)siiPrimitive.Value,
            SiiValueType.UShort when typeof(T) == typeof(ushort) => (T)siiPrimitive.Value,
            SiiValueType.Short when typeof(T) == typeof(short) => (T)siiPrimitive.Value,
            SiiValueType.UInt when typeof(T) == typeof(uint) => (T)siiPrimitive.Value,
            SiiValueType.Int when typeof(T) == typeof(int) => (T)siiPrimitive.Value,
            SiiValueType.ULong when typeof(T) == typeof(ulong) => (T)siiPrimitive.Value,
            SiiValueType.Long when typeof(T) == typeof(long) => (T)siiPrimitive.Value,
            SiiValueType.Float when typeof(T) == typeof(float) => (T)siiPrimitive.Value,
            SiiValueType.Bool when typeof(T) == typeof(bool) => (T)siiPrimitive.Value,
            SiiValueType.BlockId when typeof(T) == typeof(BlockId) => (T)siiPrimitive.Value,
            _ => throw new InvalidOperationException($"Primitive value is not of type '{typeof(T).Name}'")
        };
    }
    
    public T[] GetVector<T>(string name)
    {
        var found = _data.TryGetValue(name, out var siiValue);
        if (!found) throw new KeyNotFoundException($"Key '{name}' not found in this block");

        if (siiValue is not SiiVector siiVector)
            throw new InvalidOperationException($"Value '{name}' is not a vector type");

        try
        {
            return UnwrapVector<T>(siiVector);
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException($"Failed to unwrap vector '{name}'", e);
        }
    }

    private T[] UnwrapVector<T>(SiiVector siiVector)
    {
        return siiVector.ValueType switch
        {
            SiiValueType.Int when typeof(T) == typeof(int) => (T[])siiVector.Values,
            SiiValueType.Float when typeof(T) == typeof(float) => (T[])siiVector.Values,
            _ => throw new InvalidOperationException($"Vector values are not of type '{typeof(T).Name}'")
        };
    }

    private Array UnwrapUntypedVector(SiiVector siiVector)
    {
        try
        {
            return siiVector.ValueType switch
            {
                SiiValueType.Int => (int[])siiVector.Values,
                SiiValueType.Float => (float[])siiVector.Values,
                _ => throw new InvalidOperationException($"Vectors of type {siiVector.ValueType} are not supported")
            };
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Vector values are not of type '{siiVector.ValueType}'!");
        }
    }

    public T[] GetArray<T>(string name)
    {
        var found = _data.TryGetValue(name, out var siiValue);
        if (!found) throw new KeyNotFoundException($"Key '{name}' not found in this block");

        if (siiValue is not SiiArray siiArray)
            throw new InvalidOperationException($"Value '{name}' is not an array type");

        var values = new T[siiArray.Length];
        for (var i = 0; i < siiArray.Length; i++)
        {
            var value = siiArray.Values.GetValue(i);
            if (value is null) throw new NullReferenceException($"Value in array '{name}' is null");
            values[i] = siiArray.WrapperType switch
            {
                SiiWrapperType.Vector when typeof(T).IsArray && value is SiiVector sv =>
                    (T)(object)UnwrapUntypedVector(sv),
                SiiWrapperType.Primitive when !typeof(T).IsArray && value is SiiPrimitive sp => UnwrapPrimitive<T>(sp),
                _ => throw new InvalidOperationException($"Incompatible value in array '{name}'")
            };
        }

        return values;
    }
}