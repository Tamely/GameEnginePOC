using System.Numerics;
using GameEnginePOC.Core.Variables;
using GameEnginePOC.Core.Window;

namespace GameEnginePOC.Core.Entities;

public class Sprite2D
{
    private string SpritePath { get; set; }
    private float Scale { get; set; }
    private int Width { get; set; }
    private int Height { get; set; }
    private Vector2 Position { get; set; }
    private Bitmap Sprite { get; set; }

    public Sprite2D(string spritePath, int width, int height, Vector2 position, float scale = 1.0f)
    {
        SpritePath = spritePath;
        Scale = scale;
        Width = (int)(width * scale);
        Height = (int)(height * scale);
        Position = position;
        
        Sprite = new Bitmap(Image.FromFile(Cache.SpritesPath + SpritePath));
        
        BaseWindow.RegisterSprite(this);
    }
    
    public void SetPosition(Vector2 position) => Position = position;
    public void SetPosition(float x, float y) => Position = new Vector2(x, y);

    public Vector2 GetPosition() => Position;
    
    public void UpdateSprite(string newSpritePath) => SpritePath = newSpritePath;
    public Bitmap GetSprite() => Sprite;
    
    public void SetHeight(int height) => Height = height;
    public void SetWidth(int width) => Width = width;
    public int GetHeight() => Height;
    public int GetWidth() => Width;

    public void UpdateScale(float newScale)
    {
        float originalWidth = Width / Scale;
        float originalHeight = Height / Scale;
        
        Scale = newScale;
        
        Width = (int)(originalWidth * Scale);
        Height = (int)(originalHeight * Scale);
    }
    
    public void Destroy()
    {
        BaseWindow.DeregisterSprite(this);
    }
    
    public float GetScale() => Scale;

    public bool IsColliding(Sprite2D a, Sprite2D b)
        => a.GetPosition().X < b.GetPosition().Y + b.Width &&
           a.GetPosition().X + a.Width > b.GetPosition().X &&
           a.GetPosition().Y + a.Height > b.GetPosition().Y &&
           a.GetPosition().Y < b.GetPosition().Y + b.Height;
}