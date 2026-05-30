namespace LightHTML;

public static class ElementStyleFactory
{
    private static readonly Dictionary<(string, DisplayType, ClosingType), ElementStyle> _pool = new();

    public static int CachedCount => _pool.Count;

    public static ElementStyle Get(
        string tagName,
        DisplayType display = DisplayType.Block,
        ClosingType closing = ClosingType.WithClosing)
    {
        tagName = tagName.ToLower();
        var key = (tagName, display, closing);

        if (!_pool.TryGetValue(key, out var style))
        {
            style = new ElementStyle(tagName, display, closing);
            _pool[key] = style;
        }

        return style;
    }

    public static void Reset() => _pool.Clear();

    public static void PrintPool()
    {
        Console.WriteLine($"  Стилів у пулі Легковаговика: {_pool.Count}");
        foreach (var (key, _) in _pool)
            Console.WriteLine($"    <{key.Item1}> | {key.Item2} | {key.Item3}");
    }
}