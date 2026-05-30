namespace LightHTML;
public sealed class ElementStyle
{
    public string TagName { get; }
    public DisplayType Display { get; }
    public ClosingType Closing { get; }

    internal ElementStyle(string tagName, DisplayType display, ClosingType closing)
    {
        TagName = tagName;
        Display = display;
        Closing = closing;
    }

    public string BuildOpenTag(IReadOnlyList<string> cssClasses, bool selfClose = false)
    {
        var sb = new System.Text.StringBuilder($"<{TagName}");
        if (cssClasses.Count > 0)
            sb.Append($" class=\"{string.Join(' ', cssClasses)}\"");
        sb.Append(selfClose ? "/>" : ">");
        return sb.ToString();
    }
}