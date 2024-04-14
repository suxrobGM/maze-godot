using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class DijkstraPathfinder : IPathfinder
{
    private Grid _grid;
    private Queue<Vector2> _lastPath = new();

    public DijkstraPathfinder(Grid grid)
    {
        _grid = grid;
    }

    public IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination)
    {
        _lastPath.Clear();
        var startNode = _grid.GetNodeFromGlobalPosition(start);
        var destinationNode = _grid.GetNodeFromGlobalPosition(destination);
        var priorityQueue = new PriorityQueue<Node, int>();
        var distances = new Dictionary<Vector2, int>();
        var previous = new Dictionary<Vector2, Vector2>();

        priorityQueue.Enqueue(startNode, 0);
        distances[startNode.Position] = 0;

        while (priorityQueue.Count > 0)
        {
            var current = priorityQueue.Dequeue();

            if (current.Position == destinationNode.Position)
            {
                var path = PathfinderUtils.ConstructPath(previous, destinationNode.Position);
                
                foreach (var pos in path)
                {
                    _lastPath.Enqueue(pos);
                }
                    
                return path;
            }
            
            foreach (var neighbor in _grid.GetNeighbors(current))
            {
                // Skip unwalkable nodes
                if (neighbor.Cost == 0)
                {
                    continue; 
                }

                var newCost = distances[current.Position] + neighbor.Cost;
                
                if (!distances.ContainsKey(neighbor.Position) || newCost < distances[neighbor.Position])
                {
                    distances[neighbor.Position] = newCost;
                    priorityQueue.Enqueue(neighbor, newCost);
                    previous[neighbor.Position] = current.Position;
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
