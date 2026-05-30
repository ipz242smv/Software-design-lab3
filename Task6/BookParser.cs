using LightHTML;

namespace Task6;
static class BookParser
{
    public static LightElementNode Parse(string text)
    {
        var root = new LightElementNode("div", DisplayType.Block, ClosingType.WithClosing,
                                        new[] { "book" });

        var lines = text.Split('\n');
        bool firstContentLine = true;

        foreach (var rawLine in lines)
        {
            if (string.IsNullOrEmpty(rawLine)) continue;

            LightElementNode element;

            if (firstContentLine)
            {
                // Правило 1: перший рядок <h1>
                element = new LightElementNode("h1", DisplayType.Block, ClosingType.WithClosing);
                firstContentLine = false;
            }
            else if (rawLine.Length < 20)
            {
                // Правило 2: менше 20 символів <h2>
                element = new LightElementNode("h2", DisplayType.Block, ClosingType.WithClosing);
            }
            else if (rawLine[0] == ' ' || rawLine[0] == '\t')
            {
                // Правило 3: починається з пробілу <blockquote>
                element = new LightElementNode("blockquote", DisplayType.Block, ClosingType.WithClosing);
            }
            else
            {
                // Правило 4: все інше <p>
                element = new LightElementNode("p", DisplayType.Block, ClosingType.WithClosing);
            }

            element.AddChild(new LightTextNode(rawLine.Trim()));
            root.AddChild(element);
        }

        return root;
    }

    public static Dictionary<string, int> CollectTagStats(LightElementNode root)
    {
        var stats = new Dictionary<string, int>();
        CountTags(root, stats);
        return stats;
    }

    private static void CountTags(LightNode node, Dictionary<string, int> stats)
    {
        if (node is LightElementNode el)
        {
            stats.TryGetValue(el.TagName, out int c);
            stats[el.TagName] = c + 1;

            var field = typeof(LightElementNode)
                .GetField("_children",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(el) is List<LightNode> children)
                foreach (var ch in children) CountTags(ch, stats);
        }
    }
}