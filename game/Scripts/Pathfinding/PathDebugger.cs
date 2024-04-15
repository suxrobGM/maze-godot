using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public partial class PathDebugger : Line2D
{
    private readonly List<CollisionShape2D> markers = [];

    public void DrawPath(IEnumerable<Vector2> path)
    {
        ClearPoints();
        
        foreach (var point in path)
        {
            AddPoint(point);
        }
    }
}
