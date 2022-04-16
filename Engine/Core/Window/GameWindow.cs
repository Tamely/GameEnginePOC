using System.Numerics;
using GameEnginePOC.Core.Engine;
using GameEnginePOC.Core.Entities;

namespace GameEnginePOC.Core.Window;

public class GameWindow : BaseWindow
{
    private Sprite2D _player;
    public GameWindow(string title = "POC Game Window", int windowX = 800, int windowY = 500,
        string backgroundColor = "#FFFFFF") : base(title, (windowX, windowY), backgroundColor)
    {
        Start();
    }

    public override void OnWindowLoad()
    {
        //GraphicsWindow.KeyDown += CheckMovement;

        new GridSystem((800, 500));
        
        _player = new Sprite2D("MainPlayer.png", 10, 10, new Vector2(10, 10));
    }

    public override void OnPaint()
    {
        
    }

    public void CheckMovement(object sender, KeyEventArgs e)
    {
        var position = _player.GetPosition();
        switch (e.KeyCode)
        {
            case Keys.W:
                position.Y -= 1;
                _player.SetPosition(position);
                break;
            case Keys.D:
                position.X += 1;
                _player.SetPosition(position);
                break;
            case Keys.A:
                position.X -= 1;
                _player.SetPosition(position);
                break;
            case Keys.S:
                position.Y += 1;
                _player.SetPosition(position);
                break;
            
        }
    }
    
    int fps = 0;
    public override void OnUpdate()
    {
        Console.WriteLine(fps++);
    }
}