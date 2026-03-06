namespace TrookSii.Types.Raw;

public enum SiiValueType
{
    String,
    EncodedString,
    Byte,
    UShort,
    Short,
    UInt,
    Int,
    ULong,
    Long,
    Float,
    Bool,
    BlockId
}

public enum SiiWrapperType
{
    Primitive,
    Vector
}

public abstract class SiiValue(SiiValueType type)
{
    public SiiValueType ValueType { get; } = type;
}

public sealed class SiiPrimitive(SiiValueType t, object v) : SiiValue(t)
{
    public object Value { get; } = v;
}

public sealed class SiiVector(SiiValueType t, Array v) : SiiValue(t)
{
    public int Length { get; } = v.Length;
    public Array Values { get; } = v;
}

public sealed class SiiArray(SiiWrapperType w, SiiValueType t, SiiValue[] v) : SiiValue(t)
{
    public SiiWrapperType WrapperType { get; } = w;
    public int Length { get; } = v.Length;
    public Array Values { get; } = v;
}