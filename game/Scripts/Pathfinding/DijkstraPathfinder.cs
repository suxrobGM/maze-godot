using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class DijkstraPathfinder : GridBasedPathfinder
{
    public DijkstraPathfinder(Grid grid) : base(grid)
    {
    }
    
    public override IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = default)
    {
        options ??= new PathOptions();
        Waypoints.Clear();
        var startNode = Grid.GetNodeFromWorldPosition(start);
        var destinationNode = Grid.GetNodeFromWorldPosition(destination);
        
        var priorityQueue = new PriorityQueue<Node, int>();
        var distances = new Dictionary<Node, int>();
        var previous = new Dictionary<Node, Node>(); // for backtracking path

        priorityQueue.Enqueue(startNode, 0);
        distances[startNode] = 0;

        while (priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Dequeue();

            if (currentNode.Equals(destinationNode))
            {
                var paths = ConstructPath(previous, destinationNode, options);
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

                var newCost = distances[currentNode] + neighbor.Cost;
                
                if (!distances.ContainsKey(neighbor) || newCost < distances[neighbor])
                {
                    distances[neighbor] = newCost;
                    priorityQueue.Enqueue(neighbor, newCost);
                    previous[neighbor] = currentNode;
                }
            }
        }

        return new List<Vector2>();
    }
}
