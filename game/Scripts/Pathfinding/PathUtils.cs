using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public static class PathUtils
{
    public static void PrintPath(IEnumerable<Vector2> path)
    {
        foreach (var point in path)
        {
            GD.PrintRaw($"{point}->");
        }
        GD.Print("\n");
    }
}
