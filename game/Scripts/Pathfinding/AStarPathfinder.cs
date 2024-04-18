using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class AStarPathfinder : GridBasedPathfinder
{
    public AStarPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination)
    {
        throw new System.NotImplementedException();
    }
}
