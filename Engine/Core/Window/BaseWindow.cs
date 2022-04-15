namespace GameEnginePOC.Core.Window;

public class BaseWindow
{
    private readonly string _title;
    private readonly (int, int) _windowSize;
    private readonly (int, int, int) _backgroundColor;

    protected BaseWindow(string title, (int, int) windowSize, string backgroundColor) : this(title, windowSize, HexToRgb(backgroundColor.Replace("#",""))) {}
    protected BaseWindow(string title, (int, int) windowSize) : this(title, windowSize, (0, 0, 0)) {}
    protected BaseWindow(string title) : this(title, (800, 500), (0, 0, 0)) {}
    protected BaseWindow((int, int) windowSize) : this("POC Game Engine", windowSize, (0, 0, 0)) {}
    protected BaseWindow((int, int, int) backgroundColor) : this("POC Game Engine", (800, 500), backgroundColor) {}
    
    /// <summary>
    /// Sets all the data for the window
    /// </summary>
    /// <param name="title">The title to be displayed on the window</param>
    /// <param name="windowSize">How big the window is</param>
    /// <param name="backgroundColor">The background color of the window in RGB</param>
    private BaseWindow(string title, (int, int) windowSize, (int, int, int) backgroundColor)
    {
        _title = title;
        _windowSize = windowSize;
        _backgroundColor = backgroundColor;
    }

    /// <summary>
    /// Starts the main window with the given information in the constructors of this class.
    /// </summary>
    protected void Start()
    {
        var window = new Slate();
        window.Size = new Size(_windowSize.Item1, _windowSize.Item2);
        window.Text = _title;
        window.BackColor = Color.FromArgb(_backgroundColor.Item1, _backgroundColor.Item2, _backgroundColor.Item3);
        
        Application.Run(window);
    }

    /// <summary>
    /// Converted a hexadecimal color string to RGB. DO NOT INCLUDE THE #!
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>Triple int tuple representing RGB</returns>
    private static (int, int, int) HexToRgb(string hex)
    {
        var r = Convert.ToInt32(hex[..2], 16);
        var g = Convert.ToInt32(hex[2..4], 16);
        var b = Convert.ToInt32(hex[4..6], 16);
        return (r, g, b);
    }
}