using LightHTML;

namespace Task6;

public static class MemoryMeter
{
    private const int ObjectHeaderSize = 16;
    private const int StringHeaderSize = 20;
    private const int ListOverhead = 40;

    public static long Measure(LightNode node, HashSet<object>? visited = null)
    {
        visited ??= new HashSet<object>(ReferenceEqualityComparer.Instance);
        if (!visited.Add(node)) return 0;

        return node switch
        {
            LightTextNode t => MeasureTextNode(t),
            LightElementNode e => MeasureElementNode(e, visited),
            _ => 0
        };
    }

    private static long MeasureTextNode(LightTextNode node)
    {
        long size = ObjectHeaderSize + IntPtr.Size;
        size += MeasureString(node.OuterHTML);
        return size;
    }

    private static long MeasureElementNode(LightElementNode node, HashSet<object> visited)
    {
        long size = ObjectHeaderSize;

        size += IntPtr.Size;

        size += ListOverhead;
        foreach (var cls in node.CssClasses)
            size += IntPtr.Size + MeasureString(cls);

        size += ListOverhead;
        foreach (var child in GetChildren(node))
        {
            size += IntPtr.Size;
            size += Measure(child, visited);
        }

        return size;
    }

    public static long MeasureString(string? s)
    {
        if (s is null) return 0;
        return StringHeaderSize + s.Length * sizeof(char);
    }

    public static long MeasureElementStyleStandalone(string tagName)
    {
        return ObjectHeaderSize
             + IntPtr.Size + MeasureString(tagName)
             + 4
             + 4;
    }

    public static long MeasureFlyweightPool()
    {
        long total = 0;
        total += 48 + ElementStyleFactory.CachedCount * 32L;

        total += ElementStyleFactory.CachedCount *
                 (ObjectHeaderSize + IntPtr.Size + MeasureString("xxxx") + 4 + 4);
        return total;
    }

    private static IEnumerable<LightNode> GetChildren(LightElementNode node)
    {
        var field = typeof(LightElementNode)
            .GetField("_children",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        return field?.GetValue(node) as List<LightNode> ?? Enumerable.Empty<LightNode>();
    }
}