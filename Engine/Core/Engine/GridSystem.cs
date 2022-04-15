using GameEnginePOC.Core.Variables;

namespace GameEnginePOC.Core.Engine;

public class GridSystem
{
    private string[,] grid;
    private readonly int windowX;
    private readonly int windowY;
    
    public GridSystem((int, int) windowSize)
    {
        windowX = windowSize.Item1 / Constants.BLOCK_SIZE;
        windowY = windowSize.Item2 / Constants.BLOCK_SIZE;
        grid = new string[windowX, windowY];

        for (int i = 0; i <= windowX; i++)
        {
            for (int j = 0; j <= windowY; j++)
            {
                grid[i, j] = " ";
            }
        }
    }

    public string[,] GetGrid() => grid;
    public void SetSprite(int x, int y, string sprite) => grid[x, y] = sprite;
}