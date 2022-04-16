namespace GameEnginePOC.Core.Variables;

public class Cache
{
    public static readonly string CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GameEnginePOC";
    
    public static readonly string ShaderPath = CachePath + "\\Shaders\\";
    public static readonly string SpritesPath = CachePath + "\\Assets\\Sprites\\";
}