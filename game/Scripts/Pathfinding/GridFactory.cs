using Godot;

namespace Maze.Scripts.Pathfinding;

public static class GridFactory
{
    public static Grid CreateGridFromTileMap(TileMap tileMap)
    {
        var worldRect = tileMap.GetUsedRect();
        var width = worldRect.Size.X;
        var height = worldRect.Size.Y;
        var grid = new Grid(width, height);
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var cellPosition = new Vector2I(x + worldRect.Position.X, y + worldRect.Position.Y);
                var tile = tileMap.GetCellSourceId(0, cellPosition);
                grid[x, y] = new Node(new Vector2(cellPosition.X, cellPosition.Y), tile == 0 ? 1 : 0);
            }
        }

        return grid;
    }
}
