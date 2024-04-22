using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class BfsPathfinder : GridBasedPathfinder
{
    public BfsPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = default)
    {
        options ??= new PathOptions();
        Waypoints.Clear();
        var startNode = Grid.GetNodeFromWorldPosition(start);
        var destinationNode = Grid.GetNodeFromWorldPosition(destination);
        
        var queue = new Queue<Node>();
        var visited = new HashSet<Node>();
        var pathDict = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            
            if (currentNode.Equals(destinationNode))
            {
                var paths = ConstructPath(pathDict, destinationNode, options);
                return paths;
            }
            
            // Explore neighbors
            foreach (var neighbor in Grid.GetNeighbors(currentNode))
            {
                // Skip unwalkable nodes
                if (visited.Contains(neighbor) || neighbor.Cost == 0)
                {
                    continue;
                }

                queue.Enqueue(neighbor);
                visited.Add(neighbor);
                pathDict[neighbor] = currentNode;
            }
        }

        return new List<Vector2>();
    }
}
