using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public abstract class GridBasedPathfinder : IPathfinder
{
    public GridBasedPathfinder(Grid grid)
    {
        Grid = grid;
    }
    
    protected Grid Grid { get; }
    protected Queue<Vector2> Waypoints { get; } = new();

    public abstract IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination);

    public Vector2 GetNextPathPosition(bool convertToWorldPosition = true, bool centerToCell = true)
    {
        var path = Waypoints.Count > 0 ? Waypoints.Dequeue() : Vector2.Zero;

        if (convertToWorldPosition)
        {
            if (centerToCell) // align to center of cell
            {
                path.X = path.X * Grid.CellSize + Grid.CellSize / 2;
                path.Y = path.Y * Grid.CellSize + Grid.CellSize / 2;
            }
            else
            {
                // align to top-left corner of cell
                path.X *= Grid.CellSize;
                path.Y *= Grid.CellSize;
            }
        }
        
        return path;
    }

    public int GetPathLength()
    {
        return Waypoints.Count;
    }
}
