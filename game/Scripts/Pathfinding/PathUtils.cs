using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public static class PathUtils
{
    public static IEnumerable<Vector2> ConstructPath(Dictionary<Vector2, Vector2> path, Vector2 destination)
    {
        var sequence = new Stack<Vector2>();
        var step = destination;

        while (path.ContainsKey(step))
        {
            sequence.Push(step);
            step = path[step];
        }
        
        sequence.Push(step);  // Push the start position
        return sequence;
    }
    
    public static void PrintPath(IEnumerable<Vector2> path)
    {
        foreach (var point in path)
        {
            GD.PrintRaw($"{point}->");
        }
        GD.Print("\n");
    }
}
