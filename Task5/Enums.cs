namespace LightHTML;

public enum DisplayType
{
    Block,   // блочний
    Inline   // рядковий
}

public enum ClosingType
{
    SelfClosing,  // одиничний тег: <br/>
    WithClosing   // з закриваючим тегом: <div></div>
}