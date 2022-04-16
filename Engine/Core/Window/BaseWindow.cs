using System.Numerics;
using GameEnginePOC.Core.Entities;
using GameEnginePOC.Utilities;
using GLFW;
using static GameEnginePOC.Utilities.GL;
using Constants = GameEnginePOC.Core.Variables.Constants;
using Monitor = GLFW.Monitor;

namespace GameEnginePOC.Core.Window;

public abstract class BaseWindow
{
    private readonly string _title;
    private readonly (int, int) _windowSize;
    private readonly (float, float, float) _backgroundColor;
    private readonly GLFW.Window GraphicsWindow;
    private readonly uint ProgramContext;
    public static List<Sprite2D> CurrentSprites = new List<Sprite2D>();

    protected BaseWindow(string title, (int, int) windowSize, string backgroundColor) : this(title, windowSize, HexToRgb(backgroundColor.Replace("#",""))) {}
    protected BaseWindow(string title, (int, int) windowSize) : this(title, windowSize, (0, 0, 0)) {}
    protected BaseWindow(string title) : this(title, (800, 500), (0, 0, 0)) {}
    protected BaseWindow((int, int) windowSize) : this("POC Game Engine", windowSize, (0, 0, 0)) {}
    protected BaseWindow((float, float, float) backgroundColor) : this("POC Game Engine", (800, 500), backgroundColor) {}
    
    /// <summary>
    /// Sets all the data for the window
    /// </summary>
    /// <param name="title">The title to be displayed on the window</param>
    /// <param name="windowSize">How big the window is</param>
    /// <param name="backgroundColor">The background color of the window in RGB</param>
    private BaseWindow(string title, (int, int) windowSize, (float, float, float) backgroundColor)
    {
        _title = title;
        _windowSize = windowSize;
        _backgroundColor = backgroundColor;

        PrepareContext();
        GraphicsWindow = RenderWindow(windowSize);
        ProgramContext = CreateProgram();
    }

    /// <summary>
    /// Starts the main window with the given information in the constructors of this class.
    /// </summary>
    protected void Start()
    {
        OnWindowLoad();

        long fps = 0;
        while (!Glfw.WindowShouldClose(GraphicsWindow))
        {
            OnPaint();
            Glfw.SwapBuffers(GraphicsWindow);
            Glfw.PollEvents();

            glClear(GL_COLOR_BUFFER_BIT);
            
            if (fps++ % Constants.CLIENT_FPS == 0)
                OnUpdate();
            
            SetBackground();

            Render();
        }
        
        Glfw.Terminate();
    }

    private void SetBackground()
    {
        var vertices = new float[]
        {
            -1f, -1f, 0.0f,
            1f, -1f, 0.0f,
            -1f, 1f, 0.0f,
            1f, 1f, 0.0f
        };

        DrawRect(vertices, RgbToHex(_backgroundColor.Item1, _backgroundColor.Item2, _backgroundColor.Item3));
    }

    private void DrawRect(float[] points, string Color = "FFFFFF", bool isTextured = false,
        string texturePath = "NoTexture")
    {
        CreateVertices(points, out var vao, out _);
        
        var location = glGetUniformLocation(ProgramContext, "color");
        var color = HexToRgb(Color);
        SetColor(location, (color.Item1, color.Item2, color.Item3));
        
        
        glBindVertexArray(vao);
        glDrawArrays(GL_TRIANGLES, 0, 3);
        glDrawArrays(GL_TRIANGLES, 1, 3);
        glBindVertexArray(0);
    }

    private static unsafe void CreateVertices(float[] vertices, out uint vao, out uint vbo)
    {
        vao = glGenVertexArray();
        vbo = glGenBuffer();

        glBindVertexArray(vao);
        glBindBuffer(GL_ARRAY_BUFFER, vbo);
        fixed (float* v = &vertices[0])
        {
            glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
        }
        
        glVertexAttribPointer(0u, 3, GL_FLOAT, false, 3 * sizeof(float), null);
        glEnableVertexAttribArray(0);
    }

    /// <summary>
    /// Create and return a handle to the GLFW window using a current OpenGL context.
    /// </summary>
    /// <param name="windowSize">The size of the desired window.</param>
    /// <returns>A handle to the window.</returns>
    private GLFW.Window RenderWindow((int, int) windowSize)
    {
        // Create a window with the specified size and title
        var window = Glfw.CreateWindow(windowSize.Item1, windowSize.Item2, _title, Monitor.None, GLFW.Window.None);

        // Set the window position to center screen
        var screen = Glfw.PrimaryMonitor.WorkArea;
        var x = (screen.Width - windowSize.Item1) / 2;
        var y = (screen.Height - windowSize.Item2) / 2;
        Glfw.SetWindowPosition(window, x, y);

        // Make the window's context current
        Glfw.MakeContextCurrent(window);
        Import(Glfw.GetProcAddress);

        return window;
    }

    private void PrepareContext()
    {
        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.Decorated, true);
    }

    private void SetColor(int location, (float, float, float) RGB) => glUniform3f(location, RGB.Item1, RGB.Item2, RGB.Item3);

    private uint CreateProgram()
    {
        var vertex = CreateShader(GL_VERTEX_SHADER, @"#version 330 core
                                                    layout (location = 0) in vec3 pos;

                                                    void main()
                                                    {
                                                        gl_Position = vec4(pos.x, pos.y, pos.z, 1.0);
                                                    }");
        var fragment = CreateShader(GL_FRAGMENT_SHADER, @"#version 330 core
                                                        out vec4 result;

                                                        uniform vec3 color;

                                                        void main()
                                                        {
                                                            result = vec4(color, 1.0);
                                                        } ");
        
        var program = glCreateProgram();
        glAttachShader(program, vertex);
        glAttachShader(program, fragment);
        glLinkProgram(program);

        glDeleteShader(vertex);
        glDeleteShader(fragment);

        glUseProgram(program);

        return program;
    }

    private uint CreateShader(int type, string source)
    {
        var shader = glCreateShader(type);
        glShaderSource(shader, source);
        glCompileShader(shader);
        return shader;
    }

    private void Render()
    {
        foreach (var sprite in CurrentSprites)
        {
            //g.DrawImage(sprite.GetSprite(), sprite.GetPosition().X, sprite.GetPosition().Y, sprite.GetWidth(), sprite.GetHeight());
        }
    }

    public static void RegisterSprite(Sprite2D sprite) => CurrentSprites.Add(sprite);
    public static void DeregisterSprite(Sprite2D sprite) => CurrentSprites.Remove(sprite);

    /// <summary>
    /// Converted a hexadecimal color string to RGB. DO NOT INCLUDE THE #!
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>Triple int tuple representing RGB</returns>
    private static (float, float, float) HexToRgb(string hex)
    {
        var r = Convert.ToInt32(hex[..2], 16) / 255f;
        var g = Convert.ToInt32(hex[2..4], 16) / 255f;
        var b = Convert.ToInt32(hex[4..6], 16) / 255f;
        return (r, g, b);
    }
    
    
    /// <summary>
    /// RGB to hexadecimal color string. DOES NOT INCLUDE THE #!
    /// </summary>
    /// <param name="r">Float representing the Red value</param>
    /// <param name="g">Float representing the Green value</param>
    /// <param name="b">Float representing the Blue value</param>
    /// <returns>String representing hexadecimal color</returns>
    private static string RgbToHex(float r, float g, float b)
    {
        var rHex = Convert.ToInt32(r * 255).ToString("X2");
        var gHex = Convert.ToInt32(g * 255).ToString("X2");
        var bHex = Convert.ToInt32(b * 255).ToString("X2");
        return $"{rHex}{gHex}{bHex}";
    }
    
    /// <summary>
    /// RGB to hexadecimal color string. DOES NOT INCLUDE THE #!
    /// </summary>
    /// <param name="r">Int representing the Red value</param>
    /// <param name="g">Int representing the Green value</param>
    /// <param name="b">Int representing the Blue value</param>
    /// <returns>String representing hexadecimal color</returns>
    private static string RgbToHex(int r, int g, int b)
    {
        var rHex = Convert.ToInt32(r).ToString("X2");
        var gHex = Convert.ToInt32(g).ToString("X2");
        var bHex = Convert.ToInt32(b).ToString("X2");
        return $"{rHex}{gHex}{bHex}";
    }

    public abstract void OnWindowLoad();
    public abstract void OnPaint();
    public abstract void OnUpdate();
}