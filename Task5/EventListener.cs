namespace LightHTML;

public delegate void EventHandler(string eventName, LightElementNode sender);

public interface IEventListener
{
    void HandleEvent(string eventName, LightElementNode sender);
}