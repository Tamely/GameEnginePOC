namespace GameEnginePOC.Core.Window;

public class GameWindow : BaseWindow
{
    public GameWindow(string title = "POC Game Window", int windowX = 800, int windowY = 500,
        string backgroundColor = "#FFFFFF") : base(title, (windowX, windowY), backgroundColor)
    {
        Start();
    }
}