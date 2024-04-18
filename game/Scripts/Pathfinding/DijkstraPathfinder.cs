using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class DijkstraPathfinder : GridBasedPathfinder
{
    public DijkstraPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination)
    {
        Waypoints.Clear();
        var startNode = Grid.GetNodeFromWorldPosition(start);
        var destinationNode = Grid.GetNodeFromWorldPosition(destination);
        var priorityQueue = new PriorityQueue<Node, int>();
        var distances = new Dictionary<Vector2, int>();
        var previous = new Dictionary<Vector2, Vector2>();

        priorityQueue.Enqueue(startNode, 0);
        distances[startNode.GridPosition] = 0;

        while (priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Dequeue();

            if (currentNode.GridPosition == destinationNode.GridPosition)
            {
                var paths = PathUtils.ConstructPath(previous, destinationNode.GridPosition).ToList();
                
                foreach (var path in paths)
                {
                    Waypoints.Enqueue(path);
                }
                    
                return paths;
            }
            
            // Explore neighbors
            foreach (var neighbor in Grid.GetNeighbors(currentNode))
            {
                // Skip unwalkable nodes
                if (neighbor.Cost == 0)
                {
                    continue; 
                }

                var newCost = distances[currentNode.GridPosition] + neighbor.Cost;
                
                if (!distances.ContainsKey(neighbor.GridPosition) || newCost < distances[neighbor.GridPosition])
                {
                    distances[neighbor.GridPosition] = newCost;
                    priorityQueue.Enqueue(neighbor, newCost);
                    previous[neighbor.GridPosition] = currentNode.GridPosition;
                }
            }
        }

        return new List<Vector2>();
    }
}
