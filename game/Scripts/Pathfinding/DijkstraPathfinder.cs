using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class DijkstraPathfinder : IPathfinder
{
    private readonly Grid _grid;
    private readonly Queue<Vector2> _lastPath = new();

    public DijkstraPathfinder(Grid grid)
    {
        _grid = grid;
    }

    public IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination)
    {
        _lastPath.Clear();
        var startNode = _grid.GetNodeFromWorldPosition(start);
        var destinationNode = _grid.GetNodeFromWorldPosition(destination);
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
                    _lastPath.Enqueue(path);
                }
                    
                return paths;
            }
            
            // Explore neighbors
            foreach (var neighbor in _grid.GetNeighbors(currentNode))
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

        return new List<Vector2>(); // return an empty path if none found
    }

    public Vector2 GetNextPathPosition()
    {
        return _lastPath.Count > 0 ? _lastPath.Dequeue() : Vector2.Zero;
    }
}
