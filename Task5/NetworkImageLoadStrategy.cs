namespace LightHTML;

public class NetworkImageLoadStrategy : IImageLoadStrategy
{
    public string Load(string href)
    {
        Console.WriteLine($"[NetworkStrategy] Завантаження з мережі: {href}");
        Console.WriteLine($"[NetworkStrategy] Підключення до сервера...");
        Console.WriteLine($"[NetworkStrategy] Отримано відповідь 200 OK");

        return href;
    }
}