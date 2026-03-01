namespace TrookSii.Types.Mappings;

[AttributeUsage(AttributeTargets.Property)]
public class SiiAttribute(string siiName) : Attribute
{
    public string SiiName => siiName;
}