using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public interface IPathfinder
{
    IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination);
}
