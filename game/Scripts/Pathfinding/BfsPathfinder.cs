using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class BfsPathfinder : GridBasedPathfinder
{
    public BfsPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination)
    {
        var startNode = Grid[(int)start.X, (int)start.Y];
        var destinationNode = Grid[(int)destination.X, (int)destination.Y];
        var queue = new Queue<Node>();
        var visited = new HashSet<Vector2>();
        var path = new Dictionary<Vector2, Vector2>();

        queue.Enqueue(startNode);
        visited.Add(startNode.GridPosition);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (currentNode.GridPosition == destinationNode.GridPosition)
            {
                return PathUtils.ConstructPath(path, destinationNode.WorldPosition);
            }
            
            // Explore neighbors
            foreach (var neighbor in Grid.GetNeighbors(currentNode))
            {
                if (visited.Contains(neighbor.GridPosition) || neighbor.Cost == 0)
                {
                    continue;
                }

                queue.Enqueue(neighbor);
                visited.Add(neighbor.GridPosition);
                path[neighbor.GridPosition] = currentNode.GridPosition;
            }
        }

        return new List<Vector2>(); // return an empty path if none found
    }
}
