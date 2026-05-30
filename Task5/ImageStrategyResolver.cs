namespace LightHTML;

public static class ImageStrategyResolver
{
    public static IImageLoadStrategy Resolve(string href)
    {
        if (href.StartsWith("http://") || href.StartsWith("https://"))
            return new NetworkImageLoadStrategy();

        return new FileImageLoadStrategy();
    }
}