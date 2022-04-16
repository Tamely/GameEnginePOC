using System.Numerics;
using GameEnginePOC.Core.Entities;
using GameEnginePOC.Core.Variables;

namespace GameEnginePOC.Core.Engine;

public class GridSystem
{
    private string[,] _grid;
    private readonly int _windowX;
    private readonly int _windowY;
    private readonly int _borderXSize;
    private readonly int _borderYSize;
    
    public GridSystem((int, int) windowSize)
    {
        _windowX = (windowSize.Item1 - 10) / Constants.BLOCK_SIZE;
        _windowY = (windowSize.Item2 - 30) / Constants.BLOCK_SIZE;
        _grid = new string[_windowX, _windowY];

        _borderXSize = (windowSize.Item1 - 10) % Constants.BLOCK_SIZE / 2;
        _borderYSize = (windowSize.Item2 - 30) % Constants.BLOCK_SIZE / 2;
        
        for (int i = 0; i <= _windowX - 1; i++)
        {
            for (int j = 0; j <= _windowY - 1; j++)
            {
                _grid[i, j] = "BACKGROUND";
            }
        }
        
        StartGrid("MainPlayer.png");
    }

    public void StartGrid(string backgroundSprite)
    {
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                new Sprite2D(backgroundSprite, 
                            Constants.BLOCK_SIZE, 
                            Constants.BLOCK_SIZE, 
                            new Vector2(_borderXSize + x * Constants.BLOCK_SIZE, _borderYSize + y * Constants.BLOCK_SIZE));
            }
        }
    }

    public void DrawSpriteToGrid(int spriteX, int spriteY, Sprite2D sprite) =>
        _grid[spriteX, spriteY] = sprite.GetSpritePath();
    public string[,] GetGrid() => _grid;
    public void SetSprite(int x, int y, string sprite) => _grid[x, y] = sprite;
}