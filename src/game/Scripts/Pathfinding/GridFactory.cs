using Godot;

namespace Maze.Scripts.Pathfinding;

public static class GridFactory
{
    public static Grid CreateGridFromTileMap(TileMap tileMap)
    {
        var worldRect = tileMap.GetUsedRect();
        var grid = new Grid(worldRect.Size.X, worldRect.Size.Y);
        var walkableCells = tileMap.GetUsedCells(0);

        foreach (var cell in walkableCells)
        {
            grid[cell.X, cell.Y].Cost = 1;
        }
        
        return grid;
    }
}
