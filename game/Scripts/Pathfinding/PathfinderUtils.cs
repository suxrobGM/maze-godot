using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Maze.Scripts.Pathfinding;

public static class PathfinderUtils
{
    public static ICollection<Vector2> ConstructPath(Dictionary<Vector2, Vector2> path, Vector2 destination)
    {
        var sequence = new Stack<Vector2>();
        var step = destination;

        while (path.ContainsKey(step))
        {
            sequence.Push(step);
            step = path[step];
        }
        
        sequence.Push(step);  // Push the start position
        return sequence.ToList();
    }
}
