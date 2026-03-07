using TrookSii.Types.Blocks;
using TrookSii.Types.Raw;

namespace TrookSii.Types.Decoders.PlainText;

public class PlainSiiDecoder
{
    private readonly PlainSiiLexer _lexer;
    private PlainSiiToken _next;

    public PlainSiiDecoder(string raw)
    {
        _lexer = new PlainSiiLexer(raw);
        _next = _lexer.GetToken();
    }

    public SiiTextFile ParseFile()
    {
        var header = ReadIdentifier();
        Eat(SiiTokenKind.ObjectOpen);

        var blocks = new List<PlainBlock>();

        while (_next.Kind != SiiTokenKind.ObjectClose)
        {
            blocks.Add(ParseBlock());
        }

        Eat(SiiTokenKind.ObjectClose);

        // verify EOF
        Eat(SiiTokenKind.FileEnd);

        return new SiiTextFile(blocks);
    }

    private PlainBlock ParseBlock()
    {
        // read struct name and block id
        var structName = ReadIdentifier();
        Eat(SiiTokenKind.Colon);
        var blockIdString = ReadIdentifier();
        Eat(SiiTokenKind.ObjectOpen);
        
        // read all properties (normalize arrays post hoc)
        var properties = new List<PlainSiiRawProperty>();
        while (_next.Kind != SiiTokenKind.ObjectClose)
        {
            properties.Add(ParseProperty());
        }

        var normalizedProperties = new List<PlainSiiProperty>();
        
        // normalize arrays
        var arrayValuePropNames = properties.Where(p => p.Index is not null).Select(p => p.Name).Distinct().ToList();
        foreach (var arrayProp in arrayValuePropNames)
        {
            // find the length
            var lengthProp = properties.FirstOrDefault(p => p.Name == arrayProp && p.Index is null);
            if (lengthProp is null)
                throw new InvalidOperationException(
                    $"Array property {arrayProp} in block is missing a length component!");

            var parsed = int.TryParse(lengthProp.Value, out var l);
            if (!parsed)
                throw new InvalidOperationException($"Array length property for {arrayProp} is not a number!");

            var values = new string[l];
            for (var i = 0; i < l; i++)
            {
                var idxProp = properties.FirstOrDefault(p => p.Name == arrayProp && p.Index == i.ToString());
                if (idxProp is null)
                    throw new InvalidOperationException(
                        $"Array length is supposed to be {l} but element {i} is missing!");
                values[i] = idxProp.Value;
            }

            normalizedProperties.Add(new PlainSiiArray(arrayProp, values));
        }

        var scalarProperties = properties.Where(p => !arrayValuePropNames.Contains(p.Name))
            .Select(p => new PlainSiiScalar(p.Name, p.Value));
        normalizedProperties.AddRange(scalarProperties);
        
        Eat(SiiTokenKind.ObjectClose);

        return new PlainBlock(structName, blockIdString, normalizedProperties);
    }

    private PlainSiiRawProperty ParseProperty()
    {
        var name = ReadIdentifier();

        string? arrayIdx = null;
        if (_next.Kind == SiiTokenKind.ArrayOpen)
        {
            // grab array index
            Eat(SiiTokenKind.ArrayOpen);
            arrayIdx = ReadNumber();
            Eat(SiiTokenKind.ArrayClose);
        }

        Eat(SiiTokenKind.Colon);

        var value = _next.Kind switch
        {
            SiiTokenKind.Identifier => ReadIdentifier(),
            SiiTokenKind.Number => ReadNumber(),
            SiiTokenKind.String => ReadString(),
            _ => throw new InvalidOperationException(
                $"Expected Identifier, Number, or String token but found {_next.Kind}")
        };

        return new PlainSiiRawProperty(name, value, arrayIdx);
    }

    private string ReadIdentifier() => Read(SiiTokenKind.Identifier);
    
    private string ReadNumber() => Read(SiiTokenKind.Number);
    
    private string ReadString() => Read(SiiTokenKind.String);

    private string Read(SiiTokenKind kind) => Eat(kind).Value;

    private PlainSiiToken Eat(SiiTokenKind kind)
    {
        if (kind != _next.Kind)
            throw new InvalidOperationException(
                $"Expected token of kind {kind.ToString()} but found token of kind {_next.Kind}");

        var t = _next;
        _next = _lexer.GetToken();

        return t;
    }
}