namespace LightHTML;

public class FileImageLoadStrategy : IImageLoadStrategy
{
    public string Load(string href)
    {
        Console.WriteLine($"[FileStrategy] Завантаження з файлової системи: {href}");

        if (!File.Exists(href))
        {
            Console.WriteLine($"[FileStrategy] Файл не знайдено: {href}");
            return string.Empty;
        }

        byte[] bytes = File.ReadAllBytes(href);
        string base64 = Convert.ToBase64String(bytes);
        string ext = Path.GetExtension(href).TrimStart('.').ToLower();
        string mime = ext == "jpg" ? "jpeg" : ext;
        return $"data:image/{mime};base64,{base64[..Math.Min(30, base64.Length)]}...";
    }
}