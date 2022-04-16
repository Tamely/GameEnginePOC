using GameEnginePOC.Core.Entities;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace GameEnginePOC.Core.Window;

public abstract class BaseWindow : GameWindow
{
    public static List<Sprite2D> CurrentSprites = new List<Sprite2D>();

    /// <summary>
    /// Sets all the data for the window
    /// </summary>
    /// <param name="title">The title to be displayed on the window</param>
    /// <param name="windowSize">How big the window is</param>
    protected BaseWindow(string title, (int, int) windowSize) : base(GameWindowSettings.Default, new NativeWindowSettings()
    {
        Size = new Vector2i(windowSize.Item1, windowSize.Item2),
        Title = title,
        Flags = ContextFlags.ForwardCompatible
    }) {}

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        
        base.OnUpdateFrame(e);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        OnWindowLoad();
    }

    protected override void OnRenderFrame(FrameEventArgs eventArgs)
    {
        base.OnRenderFrame(eventArgs);
        OnRenderFrame();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        OnResize();
    }

    public static void RegisterSprite(Sprite2D sprite) => CurrentSprites.Add(sprite);
    public static void DeregisterSprite(Sprite2D sprite) => CurrentSprites.Remove(sprite);

    protected abstract void OnWindowLoad();
    protected abstract void OnRenderFrame();
    protected abstract void OnResize();

}