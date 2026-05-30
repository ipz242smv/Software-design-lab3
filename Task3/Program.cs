public interface IRenderEngine
{
    void RenderCircle();
    void RenderSquare();
    void RenderTriangle();
}

public class VectorRenderEngine : IRenderEngine
{
    public void RenderCircle()
    {
        Console.WriteLine("Drawing Circle as vector graphics");
    }

    public void RenderSquare()
    {
        Console.WriteLine("Drawing Square as vector graphics");
    }

    public void RenderTriangle()
    {
        Console.WriteLine("Drawing Triangle as vector graphics");
    }
}

public class RasterRenderEngine : IRenderEngine
{
    public void RenderCircle()
    {
        Console.WriteLine("Drawing Circle as pixels");
    }

    public void RenderSquare()
    {
        Console.WriteLine("Drawing Square as pixels");
    }

    public void RenderTriangle()
    {
        Console.WriteLine("Drawing Triangle as pixels");
    }
}

public abstract class Shape
{
    protected IRenderEngine _renderEngine;

    protected Shape(IRenderEngine renderEngine)
    {
        _renderEngine = renderEngine;
    }

    public abstract void Draw();

    public void SetRenderEngine(IRenderEngine renderEngine)
    {
        _renderEngine = renderEngine;
    }
}

public class Circle : Shape
{
    private double _radius;

    public Circle(IRenderEngine renderEngine, double radius) : base(renderEngine)
    {
        _radius = radius;
    }

    public override void Draw()
    {
        Console.Write($"Circle (radius: {_radius}) - ");
        _renderEngine.RenderCircle();
    }
}

public class Square : Shape
{
    private double _side;

    public Square(IRenderEngine renderEngine, double side) : base(renderEngine)
    {
        _side = side;
    }

    public override void Draw()
    {
        Console.Write($"Square (side: {_side}) - ");
        _renderEngine.RenderSquare();
    }
}

public class Triangle : Shape
{
    private double _baseLength;
    private double _height;

    public Triangle(IRenderEngine renderEngine, double baseLength, double height) : base(renderEngine)
    {
        _baseLength = baseLength;
        _height = height;
    }

    public override void Draw()
    {
        Console.Write($"Triangle (base: {_baseLength}, height: {_height}) - ");
        _renderEngine.RenderTriangle();
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IRenderEngine vectorEngine = new VectorRenderEngine();
        IRenderEngine rasterEngine = new RasterRenderEngine();

        Console.WriteLine("1. ВЕКТОРНИЙ РЕНДЕРИНГ");

        Shape circle1 = new Circle(vectorEngine, 5.5);
        Shape square1 = new Square(vectorEngine, 4.0);
        Shape triangle1 = new Triangle(vectorEngine, 6.0, 3.0);

        circle1.Draw();
        square1.Draw();
        triangle1.Draw();

        Console.WriteLine();

        Console.WriteLine("2. РАСТРОВИЙ РЕНДЕРИНГ");

        Shape circle2 = new Circle(rasterEngine, 3.2);
        Shape square2 = new Square(rasterEngine, 5.0);
        Shape triangle2 = new Triangle(rasterEngine, 4.5, 2.5);

        circle2.Draw();
        square2.Draw();
        triangle2.Draw();

        Console.WriteLine();

        Console.WriteLine("3. ДИНАМІЧНА ЗМІНА РЕНДЕРИНГУ");

        Shape dynamicShape = new Circle(vectorEngine, 10.0);

        Console.WriteLine("Початковий стан:");
        dynamicShape.Draw();

        Console.WriteLine("\nЗмінюємо на растровий рендеринг:");
        dynamicShape.SetRenderEngine(rasterEngine);
        dynamicShape.Draw();

        Console.WriteLine("\nЗмінюємо назад на векторний:");
        dynamicShape.SetRenderEngine(vectorEngine);
        dynamicShape.Draw();

        Console.WriteLine();

        Console.WriteLine("4. ВСІ ФІГУРИ З ОБОМА ТИПАМИ РЕНДЕРИНГУ");

        Shape[] shapes = new Shape[]
        {
            new Circle(vectorEngine, 2.5),
            new Square(vectorEngine, 3.0),
            new Triangle(vectorEngine, 5.0, 4.0),
            new Circle(rasterEngine, 7.0),
            new Square(rasterEngine, 6.0),
            new Triangle(rasterEngine, 8.0, 5.0)
        };

        Console.WriteLine("Векторний рендеринг:");
        for (int i = 0; i < 3; i++)
        {
            shapes[i].Draw();
        }

        Console.WriteLine("\nРастровий рендеринг:");
        for (int i = 3; i < 6; i++)
        {
            shapes[i].Draw();
        }

        Console.WriteLine();
        Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
        Console.ReadKey();
    }
}