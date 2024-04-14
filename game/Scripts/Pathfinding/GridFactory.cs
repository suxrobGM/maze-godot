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
        var walkableCells = tileMap.GetUsedCells(0);

        foreach (var cell in walkableCells)
        {
            grid[cell.X, cell.Y].Cost = 1;
        }
        
        return grid;
    }
}
