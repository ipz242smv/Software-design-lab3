namespace LightHTML;

public class LightImageNode : LightNode
{
    private readonly string _href;
    private readonly string _alt;
    private readonly IImageLoadStrategy _strategy;
    private string? _loadedSrc;

    public string Href => _href;
    public string Alt => _alt;

    public LightImageNode(string href, string alt = "", IImageLoadStrategy? strategy = null)
    {
        if (string.IsNullOrWhiteSpace(href))
            throw new ArgumentException("href cannot be empty.", nameof(href));

        _href = href;
        _alt = alt;
        _strategy = strategy ?? ImageStrategyResolver.Resolve(href);
    }

    public string Load()
    {
        _loadedSrc = _strategy.Load(_href);
        return _loadedSrc;
    }

    public override string OuterHTML
    {
        get
        {
            string src = _loadedSrc ?? _href;
            string alt = string.IsNullOrEmpty(_alt) ? "" : $" alt=\"{_alt}\"";
            return $"<img src=\"{src}\"{alt}/>";
        }
    }

    public override string InnerHTML => string.Empty;
}