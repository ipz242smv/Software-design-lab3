using System.Text;

namespace LightHTML;

public class LightElementNode : LightNode
{
    private readonly ElementStyle _style;
    private readonly List<string> _cssClasses = new();
    private readonly List<LightNode> _children = new();

    private readonly Dictionary<string, List<IEventListener>> _listeners = new();

    public string TagName => _style.TagName;
    public DisplayType Display => _style.Display;
    public ClosingType Closing => _style.Closing;
    public IReadOnlyList<string> CssClasses => _cssClasses.AsReadOnly();
    public int ChildCount => _children.Count;

    public LightElementNode(
        string tagName,
        DisplayType display = DisplayType.Block,
        ClosingType closing = ClosingType.WithClosing,
        IEnumerable<string>? cssClasses = null)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            throw new ArgumentException("Tag name cannot be empty.", nameof(tagName));

        _style = ElementStyleFactory.Get(tagName, display, closing);

        if (cssClasses is not null)
            _cssClasses.AddRange(cssClasses);
    }


    public void AddEventListener(string eventName, IEventListener listener)
    {
        if (!_listeners.TryGetValue(eventName, out var list))
        {
            list = new List<IEventListener>();
            _listeners[eventName] = list;
        }
        list.Add(listener);
    }

    public void RemoveEventListener(string eventName, IEventListener listener)
    {
        if (_listeners.TryGetValue(eventName, out var list))
            list.Remove(listener);
    }

    public void DispatchEvent(string eventName)
    {
        if (_listeners.TryGetValue(eventName, out var list))
            foreach (var listener in list)
                listener.HandleEvent(eventName, this);
    }

    public LightElementNode AddChild(LightNode node)
    {
        if (Closing == ClosingType.SelfClosing)
            throw new InvalidOperationException(
                $"Self-closing tag <{TagName}/> cannot have children.");

        _children.Add(node ?? throw new ArgumentNullException(nameof(node)));
        return this;
    }

    public LightElementNode AddClass(string cssClass)
    {
        if (!string.IsNullOrWhiteSpace(cssClass))
            _cssClasses.Add(cssClass.Trim());
        return this;
    }

    public override string InnerHTML
    {
        get
        {
            if (Closing == ClosingType.SelfClosing || _children.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var child in _children)
                sb.Append(child.OuterHTML);
            return sb.ToString();
        }
    }

    public override string OuterHTML
    {
        get
        {
            if (Closing == ClosingType.SelfClosing)
                return _style.BuildOpenTag(_cssClasses, selfClose: true);

            return $"{_style.BuildOpenTag(_cssClasses)}{InnerHTML}</{TagName}>";
        }
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Tag      : <{TagName}>");
        Console.WriteLine($"Display  : {Display}");
        Console.WriteLine($"Closing  : {Closing}");
        Console.WriteLine($"Classes  : {(CssClasses.Count > 0 ? string.Join(", ", CssClasses) : "(none)")}");
        Console.WriteLine($"Children : {ChildCount}");
    }
}