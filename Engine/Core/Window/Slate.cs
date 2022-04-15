namespace GameEnginePOC.Core.Window;

/// <summary>
/// Base window for the game engine to run in. 
/// </summary>
/// Extends the <see cref="GameEnginePOC.Core.Window.WindowBase"/> class which is not ideal, but this is only a POC.
sealed class Slate : Form
{
    /// <summary>
    /// Initializes a smooth display.
    /// </summary>
    public Slate()
    {
        // Prevents flickering when refreshing the form
        DoubleBuffered = true;
    }
}