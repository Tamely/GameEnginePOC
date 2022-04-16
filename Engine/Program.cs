using GameEnginePOC.Core.Window;

namespace GameEnginePOC;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Initialized the console.");
        BaseWindow window = new EngineWindow("#FFFFFF");
    }
}