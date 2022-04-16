using GameEnginePOC.Core.Variables;
using GameEnginePOC.Utilities;
using OpenTK.Graphics.OpenGL4;



namespace GameEnginePOC.Core.Window;

public sealed class EngineWindow : BaseWindow
{
    private readonly (float, float, float) _backgroundColor;
    private float[] _vertices = new float[]
    {
        -0.5f, -0.5f, 0.0f,
        0.5f, -0.5f, 0.0f,
        0.0f, 0.5f, 0.0f
    };

    private Shader _shader;
    
    private int _vertexBufferObject;
    private int _vertexArrayObject;
    public EngineWindow(string backgroundColor, int width = 800, int height = 500, string title = "POC Game Engine") : this(HexToRgb(backgroundColor), width, height, title) {}
    public EngineWindow((float, float, float) backgroundColor, int width = 800, int height = 500, string title = "POC Game Engine") : base(title, (width, height))
    {
        _backgroundColor = backgroundColor;
        Run();
    }

    protected override void OnWindowLoad()
    {
        GL.ClearColor(_backgroundColor.Item1, _backgroundColor.Item2, _backgroundColor.Item3, 1.0f);
        
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        
        GL.EnableVertexAttribArray(0);
        
        _shader = new Shader(Cache.ShaderPath + "shader.vert", Cache.ShaderPath + "shader.frag");
        _shader.Use();
        
    }

    protected override void OnRenderFrame()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        SwapBuffers();
    }

    protected override void OnResize()
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
    }

    /// <summary>
    /// Converted a hexadecimal color string to RGB.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns>Triple int tuple representing RGB</returns>
    private static (float, float, float) HexToRgb(string hex)
    {
        string newHex = hex.Replace("#", "");
        var r = Convert.ToInt32(newHex[..2], 16) / 255f;
        var g = Convert.ToInt32(newHex[2..4], 16) / 255f;
        var b = Convert.ToInt32(newHex[4..6], 16) / 255f;
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
}