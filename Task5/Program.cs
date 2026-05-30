using LightHTML;
using System.Text;

Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;


PrintSeparator("1. ІНФОРМАЦІЯ ПРО ЕЛЕМЕНТ <div>");

var divInfo = new LightElementNode("div", DisplayType.Block, ClosingType.WithClosing,
                                   new[] { "container", "main" });
divInfo.AddChild(new LightTextNode("Hello, LightHTML!"));
divInfo.PrintInfo();

PrintSeparator("2. САМО-ЗАКРИВАЮЧИЙ ТЕГ <br/>");

var br = new LightElementNode("br", DisplayType.Block, ClosingType.SelfClosing);
Console.WriteLine($"OuterHTML: {br.OuterHTML}");


PrintSeparator("3. НЕВПОРЯДКОВАНИЙ СПИСОК <ul>");

/*
  <ul class="nav-list">
    <li class="nav-item"><a class="nav-link">Головна</a></li>
    <li class="nav-item"><a class="nav-link">Про нас</a></li>
    <li class="nav-item"><a class="nav-link">Контакти</a></li>
  </ul>
*/

var items = new[] { "Головна", "Про нас", "Контакти" };
var ul = new LightElementNode("ul", DisplayType.Block, ClosingType.WithClosing,
                               new[] { "nav-list" });

foreach (var text in items)
{
    var a = new LightElementNode("a", DisplayType.Inline, ClosingType.WithClosing,
                                  new[] { "nav-link" })
                .AddChild(new LightTextNode(text));

    var li = new LightElementNode("li", DisplayType.Block, ClosingType.WithClosing,
                                   new[] { "nav-item" })
                 .AddChild(a);

    ul.AddChild(li);
}

Console.WriteLine("── outerHTML ──");
Console.WriteLine(PrettyPrint(ul.OuterHTML));
Console.WriteLine($"\nДочірніх елементів: {ul.ChildCount}");


PrintSeparator("4. ТАБЛИЦЯ СТУДЕНТІВ <table>");

/*
  <table class="students">
    <thead>
      <tr>
        <th>№</th><th>Ім'я</th><th>Оцінка</th>
      </tr>
    </thead>
    <tbody>
      <tr><td>1</td><td>Олена</td><td>95</td></tr>
      <tr><td>2</td><td>Максим</td><td>88</td></tr>
      <tr><td>3</td><td>Соломія</td><td>92</td></tr>
    </tbody>
  </table>
*/

LightElementNode MakeCell(string tag, string content) =>
    new LightElementNode(tag, DisplayType.Block, ClosingType.WithClosing)
        .AddChild(new LightTextNode(content));

LightElementNode MakeRow(params string[] cells) =>
    cells.Aggregate(
        new LightElementNode("tr", DisplayType.Block, ClosingType.WithClosing),
        (tr, cell) => tr.AddChild(MakeCell("td", cell)));

var headerRow = new LightElementNode("tr", DisplayType.Block, ClosingType.WithClosing);
foreach (var h in new[] { "№", "Ім'я", "Оцінка" })
    headerRow.AddChild(MakeCell("th", h));

var thead = new LightElementNode("thead", DisplayType.Block, ClosingType.WithClosing)
                .AddChild(headerRow);

string[][] rows =
[
    ["1", "Олена",   "95"],
    ["2", "Максим",  "88"],
    ["3", "Соломія", "92"],
];

var tbody = new LightElementNode("tbody", DisplayType.Block, ClosingType.WithClosing);
foreach (var row in rows)
    tbody.AddChild(MakeRow(row));

var table = new LightElementNode("table", DisplayType.Block, ClosingType.WithClosing,
                                  new[] { "students" })
                .AddChild(thead)
                .AddChild(tbody);

Console.WriteLine("── outerHTML ──");
Console.WriteLine(PrettyPrint(table.OuterHTML));

Console.WriteLine("\n── innerHTML ──");
Console.WriteLine(PrettyPrint(table.InnerHTML));

Console.WriteLine($"\nДочірніх елементів <table>: {table.ChildCount}");
Console.WriteLine($"Дочірніх елементів <tbody>: {tbody.ChildCount}");


PrintSeparator("5. ПОМИЛКА: додавання дочірніх до само-закриваючого тегу");

try
{
    var img = new LightElementNode("img", DisplayType.Inline, ClosingType.SelfClosing);
    img.AddChild(new LightTextNode("not allowed"));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[очікувана помилка] {ex.Message}");
}

PrintSeparator("6. СПОСТЕРІГАЧ: EventListener");

var button = new LightElementNode("button", DisplayType.Inline, ClosingType.WithClosing);
button.AddChild(new LightTextNode("Натисни мене"));

var logger1 = new ConsoleEventListener("Logger1");
var logger2 = new ConsoleEventListener("Logger2");
var analytics = new ConsoleEventListener("Analytics");

button.AddEventListener("click", logger1);
button.AddEventListener("click", logger2);
button.AddEventListener("mouseover", analytics);
button.AddEventListener("mouseout", analytics);

Console.WriteLine("Симулюємо click");
button.DispatchEvent("click");

Console.WriteLine("Симулюємо mouseover");
button.DispatchEvent("mouseover");

Console.WriteLine("Симулюємо mouseout");
button.DispatchEvent("mouseout");

Console.WriteLine("Відписуємо Logger2 від click");
button.RemoveEventListener("click", logger2);
button.DispatchEvent("click");

static void PrintSeparator(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('═', 60));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('═', 60));
}

static string PrettyPrint(string html)
{
    var sb = new System.Text.StringBuilder();
    int indent = 0;
    int i = 0;

    while (i < html.Length)
    {
        if (html[i] == '<')
        {
            int end = html.IndexOf('>', i);
            if (end == -1) { sb.Append(html[i++]); continue; }

            string tag = html.Substring(i, end - i + 1);
            bool isClose = tag.StartsWith("</");
            bool isSelfClose = tag.EndsWith("/>");

            if (isClose) indent = Math.Max(0, indent - 1);

            sb.AppendLine();
            sb.Append(new string(' ', indent * 2));
            sb.Append(tag);

            if (!isClose && !isSelfClose) indent++;
            i = end + 1;
        }
        else
        {
            int next = html.IndexOf('<', i);
            string text = next == -1 ? html[i..] : html[i..next];
            if (!string.IsNullOrWhiteSpace(text)) sb.Append(text.Trim());
            i = next == -1 ? html.Length : next;
        }
    }

    return sb.ToString().TrimStart('\r', '\n');
}