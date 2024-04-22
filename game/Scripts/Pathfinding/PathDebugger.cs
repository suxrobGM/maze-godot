using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public partial class PathDebugger : Line2D
{
    public void DrawPath(IEnumerable<Vector2> path)
    {
        ClearPoints();
        
        foreach (var point in path)
        {
            AddPoint(point);
        }
    }
}
