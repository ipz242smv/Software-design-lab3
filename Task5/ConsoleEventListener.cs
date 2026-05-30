namespace LightHTML;

public class ConsoleEventListener : IEventListener
{
    private readonly string _name;

    public ConsoleEventListener(string name)
    {
        _name = name;
    }

    public void HandleEvent(string eventName, LightElementNode sender)
    {
        Console.WriteLine($"[{_name}] Подія '{eventName}' спрацювала на елементі <{sender.TagName}>");
    }
}