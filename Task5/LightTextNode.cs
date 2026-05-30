namespace LightHTML;

public class LightTextNode : LightNode
{
    private readonly string _text;

    public LightTextNode(string text)
    {
        _text = text ?? string.Empty;
    }

    public override string OuterHTML => _text;
    public override string InnerHTML => _text;
}