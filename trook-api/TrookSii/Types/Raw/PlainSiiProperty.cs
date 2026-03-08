namespace TrookSii.Types.Raw;

public record PlainSiiRawProperty(string Name, string Value, string? Index);

public enum PlainSiiPropertyType { Scalar, Array }
public abstract record PlainSiiProperty(PlainSiiPropertyType PropertyType, string Name);

public record PlainSiiScalar(string Name, string Value) : PlainSiiProperty(PlainSiiPropertyType.Scalar, Name);

public record PlainSiiArray(string Name, string[] Values) :  PlainSiiProperty(PlainSiiPropertyType.Array, Name);