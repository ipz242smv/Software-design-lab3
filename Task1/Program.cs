public class Logger
{
    public void Log(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[LOG] {message}");
        Console.ForegroundColor = originalColor;
    }

    public void Error(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ForegroundColor = originalColor;
    }

    public void Warn(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"[WARN] {message}");
        Console.ForegroundColor = originalColor;
    }
}
public class FileWriter
{
    private readonly string _filePath;

    public FileWriter(string filePath)
    {
        _filePath = filePath;
    }

    public void Write(string text)
    {
        File.AppendAllText(_filePath, text);
    }

    public void WriteLine(string text)
    {
        File.AppendAllText(_filePath, text + Environment.NewLine);
    }
}

public interface IFileLogger
{
    void LogMessage(string message);
    void LogError(string message);
    void LogWarning(string message);
}

public class FileLoggerAdapter : IFileLogger
{
    private readonly Logger _consoleLogger;
    private readonly FileWriter _fileWriter;

    public FileLoggerAdapter(Logger consoleLogger, FileWriter fileWriter)
    {
        _consoleLogger = consoleLogger;
        _fileWriter = fileWriter;
    }

    public void LogMessage(string message)
    {
        _consoleLogger.Log(message);
        _fileWriter.WriteLine($"[LOG] {message}");
    }

    public void LogError(string message)
    {
        _consoleLogger.Error(message);
        _fileWriter.WriteLine($"[ERROR] {message}");
    }

    public void LogWarning(string message)
    {
        _consoleLogger.Warn(message);
        _fileWriter.WriteLine($"[WARN] {message}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Демонстрація роботи файлового логера");
        Console.WriteLine();

        Console.Write("Введіть назву файлу для збереження логів: ");
        string fileName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = "log.txt";
        }

        Logger logger = new Logger();
        FileWriter fileWriter = new FileWriter(fileName);

        IFileLogger fileLogger = new FileLoggerAdapter(logger, fileWriter);

        Console.WriteLine("\nЗапис повідомлень");

        Console.Write("Введіть повідомлення для LOG: ");
        string logMsg = Console.ReadLine();
        fileLogger.LogMessage(logMsg);

        Console.Write("Введіть повідомлення для ERROR: ");
        string errorMsg = Console.ReadLine();
        fileLogger.LogError(errorMsg);

        Console.Write("Введіть повідомлення для WARNING: ");
        string warnMsg = Console.ReadLine();
        fileLogger.LogWarning(warnMsg);

        Console.WriteLine("\nПовідомлення виведені в консоль та збережені у файл '{0}'", fileName);

        // Показуємо вміст файлу
        Console.WriteLine("\nВміст файлу {0}", fileName);
        Console.WriteLine(File.ReadAllText(fileName));

        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }
}