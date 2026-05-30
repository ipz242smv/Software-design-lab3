namespace LightHTML;

public abstract class LightNode
{
    public abstract string OuterHTML { get; }
    public abstract string InnerHTML { get; }

    public override string ToString() => OuterHTML;
}