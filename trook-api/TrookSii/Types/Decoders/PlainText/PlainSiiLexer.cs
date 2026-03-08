namespace TrookSii.Types.Decoders.PlainText;

public enum SiiTokenKind
{
    Identifier,
    Number,
    String,
    Colon,
    ObjectOpen,
    ObjectClose,
    ArrayOpen,
    ArrayClose,
    FileEnd
}

public record PlainSiiToken(SiiTokenKind Kind, string Value);

public class PlainSiiLexer(string raw)
{
    private int _idx = 0;

    public PlainSiiToken GetToken()
    {
        // advance to either next non-whitespace or end of file
        EatWhitespace();
        
        // detect EOF
        if (_idx >= raw.Length)
            return new PlainSiiToken(SiiTokenKind.FileEnd, "");
        
        var c = raw[_idx];
        switch (c)
        {
            case ':': return ReadSingleCharToken(SiiTokenKind.Colon, c);
            case '[': return ReadSingleCharToken(SiiTokenKind.ArrayOpen, c);
            case ']': return ReadSingleCharToken(SiiTokenKind.ArrayClose, c);
            case '{': return ReadSingleCharToken(SiiTokenKind.ObjectOpen, c);
            case '}': return ReadSingleCharToken(SiiTokenKind.ObjectClose, c);
        }
        
        // else get longer ones
        if (c == '"')
            return ReadStringToken();
        
        if (char.IsDigit(c))
            return ReadNumberToken();
        
        // it's probably an identifier
        return ReadIdentifierToken();
    }

    private void EatWhitespace()
    {
        while (_idx < raw.Length && char.IsWhiteSpace(raw[_idx]))
            _idx++;
    }

    private PlainSiiToken ReadSingleCharToken(SiiTokenKind kind, char c)
    {
        _idx++;
        return new PlainSiiToken(kind, c.ToString());
    }

    private PlainSiiToken ReadIdentifierToken()
    {
        // assuming it's pretty freeform. any non-whitespace sequence that doesn't start with a brace, quote, or digit
        var start = _idx;
        
        // read any known supported chars
        while (char.IsLetterOrDigit(raw[_idx]) || raw[_idx] is '_' or '&' or '.')
            _idx++;

        return new PlainSiiToken(SiiTokenKind.Identifier, raw[start.._idx]);
    }

    private PlainSiiToken ReadStringToken()
    {
        // eat the opening quote
        _idx++;
        var start = _idx;
        
        // read until closing quote
        while (raw[_idx] != '"')
            _idx++;
        var end = _idx;
        
        // eat the closing quote
        _idx++;
        
        return new PlainSiiToken(SiiTokenKind.String, raw[start..end]);
    }

    private PlainSiiToken ReadNumberToken()
    {
        var start = _idx;
        while (char.IsDigit(raw[_idx]))
            _idx++;

        return new PlainSiiToken(SiiTokenKind.Number, raw[start.._idx]);
    }
}