using System.Text.RegularExpressions;
public interface ITextReader
{
    char[,] ReadFile(string filePath);
}

public class SmartTextReader : ITextReader
{
    public char[,] ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл {filePath} не знайдено");
        }

        string[] lines = File.ReadAllLines(filePath);

        int maxLength = 0;
        foreach (string line in lines)
        {
            if (line.Length > maxLength)
                maxLength = line.Length;
        }

        char[,] result = new char[lines.Length, maxLength];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                result[i, j] = lines[i][j];
            }
        }

        return result;
    }
}

public class SmartTextChecker : ITextReader
{
    private SmartTextReader _reader;

    public SmartTextChecker()
    {
        _reader = new SmartTextReader();
    }

    public char[,] ReadFile(string filePath)
    {
        Console.WriteLine($"\n--- Логування: початок роботи з файлом ---");
        Console.WriteLine($"Спроба відкрити файл: {filePath}");

        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"ПОМИЛКА: Файл {filePath} не існує");
                throw new FileNotFoundException();
            }

            Console.WriteLine($"Файл успішно відкрито: {filePath}");

            Console.WriteLine($"Читання файлу...");
            char[,] content = _reader.ReadFile(filePath);

            int rows = content.GetLength(0);
            int totalChars = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < content.GetLength(1); j++)
                {
                    if (content[i, j] != '\0')
                        totalChars++;
                }
            }

            Console.WriteLine($"Файл успішно прочитано");
            Console.WriteLine($"Статистика: {rows} рядків, {totalChars} символів");
            Console.WriteLine($"Закриття файлу: {filePath}");
            Console.WriteLine($"Логування завершено\n");

            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ПОМИЛКА під час роботи з файлом: {ex.Message}");
            throw;
        }
    }
}

public class SmartTextReaderLocker : ITextReader
{
    private SmartTextReader _reader;
    private Regex _accessRestrictionPattern;

    public SmartTextReaderLocker(string restrictionPattern)
    {
        _reader = new SmartTextReader();
        _accessRestrictionPattern = new Regex(restrictionPattern);
    }

    public char[,] ReadFile(string filePath)
    {
        if (_accessRestrictionPattern.IsMatch(filePath))
        {
            Console.WriteLine($"\n[ДОСТУП ЗАБОРОНЕНО] Доступ до файлу {filePath} обмежено!");
            Console.WriteLine("Access denied!\n");
            return null;
        }

        Console.WriteLine($"\n[ДОСТУП ДОЗВОЛЕНО] Читання файлу {filePath}...");
        return _reader.ReadFile(filePath);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        CreateTestFiles();

        Console.WriteLine("1. SMART TEXT READER ");
        ITextReader directReader = new SmartTextReader();
        char[,] directContent = directReader.ReadFile("test1.txt");
        DisplayFileContent(directContent);

        Console.WriteLine("\n2. SMART TEXT CHECKER");
        ITextReader loggerProxy = new SmartTextChecker();
        char[,] loggedContent = loggerProxy.ReadFile("test2.txt");
        DisplayFileContent(loggedContent);

        Console.WriteLine("\n3. SMART TEXT READER LOCKER");

        // Забороняємо файли, які містять "secret" в назві
        ITextReader lockerProxy = new SmartTextReaderLocker(@"(?i)secret");

        // Спроба прочитати дозволений файл
        Console.WriteLine("Спроба прочитати дозволений файл (test1.txt):");
        char[,] allowedContent = lockerProxy.ReadFile("test1.txt");
        if (allowedContent != null)
            DisplayFileContent(allowedContent);

        // Спроба прочитати заборонений файл
        Console.WriteLine("\nСпроба прочитати заборонений файл (secret_data.txt):");
        char[,] deniedContent = lockerProxy.ReadFile("secret_data.txt");
        if (deniedContent != null)
            DisplayFileContent(deniedContent);

        Console.WriteLine("\n--- 4. РІЗНІ ПАТЕРНИ ОБМЕЖЕННЯ ---");

        // Забороняємо файли з розширенням .conf
        ITextReader configLocker = new SmartTextReaderLocker(@"\.conf$");
        Console.WriteLine("Обмеження: файли з розширенням .conf");
        Console.WriteLine("Спроба прочитати config.conf:");
        configLocker.ReadFile("config.conf");

        // Забороняємо файли, що починаються з "private_"
        ITextReader privateLocker = new SmartTextReaderLocker(@"^private_");
        Console.WriteLine("\nОбмеження: файли, що починаються з 'private_'");
        Console.WriteLine("Спроба прочитати private_log.txt:");
        privateLocker.ReadFile("private_log.txt");
        Console.WriteLine("Спроба прочитати public_log.txt:");
        privateLocker.ReadFile("public_log.txt");

        Console.WriteLine("\n--- 5. КОМБІНОВАНЕ ВИКОРИСТАННЯ (кілька проксі) ---");

        // Спершу обмеження доступу, потім логування
        ITextReader restrictedLogger = new SmartTextReaderLocker(@"\.log$");

        Console.WriteLine("Читання файлу з комбінованим проксі (лог-файл):");
        char[,] combinedResult = ((SmartTextReaderLocker)restrictedLogger).ReadFile("application.log");

        Console.WriteLine("\nЧитання звичайного файлу через SmartTextChecker:");
        ITextReader justLogger = new SmartTextChecker();
        justLogger.ReadFile("test1.txt");

        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }

    static void CreateTestFiles()
    {
        File.WriteAllLines("test1.txt", new string[]
        {
            "Hello World",
            "This is test file",
            "Line 3"
        });

        File.WriteAllLines("test2.txt", new string[]
        {
            "Lorem ipsum dolor sit amet",
            "Consectetur adipiscing elit",
            "Sed do eiusmod tempor incididunt"
        });

        File.WriteAllLines("secret_data.txt", new string[]
        {
            "TOP SECRET",
            "Password: 12345",
            "Confidential data"
        });

        File.WriteAllLines("config.conf", new string[]
        {
            "[Database]",
            "Host=localhost",
            "Port=1234"
        });

        File.WriteAllLines("private_log.txt", new string[]
        {
            "ERROR: Connection failed",
            "WARN: High memory usage"
        });

        File.WriteAllLines("public_log.txt", new string[]
        {
            "INFO: User logged in",
            "INFO: File saved"
        });

        File.WriteAllLines("application.log", new string[]
        {
            "2026-01-01 10:00:00 START",
            "2026-01-01 10:00:05 PROCESSING",
            "2026-01-01 10:00:10 END"
        });

        Console.WriteLine("Тестові файли успішно створено!\n");
    }

    static void DisplayFileContent(char[,] content)
    {
        if (content == null)
        {
            Console.WriteLine("Вміст файлу: Немає даних");
            return;
        }

        Console.WriteLine("Вміст файлу:");
        int rows = content.GetLength(0);
        int cols = content.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            Console.Write($"Рядок {i + 1}: ");
            for (int j = 0; j < cols; j++)
            {
                if (content[i, j] != '\0')
                    Console.Write(content[i, j]);
            }
            Console.WriteLine();
        }
    }
}