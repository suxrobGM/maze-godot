using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public abstract class GridBasedPathfinder : IPathfinder
{
    /// <summary>
    /// The threshold distance to consider the entity has reached the waypoint.
    /// </summary>
    private const float WaypointReachedThreshold = 5f;
    
    protected GridBasedPathfinder(Grid grid)
    {
        Grid = grid;
    }
    
    protected Grid Grid { get; }
    protected Queue<Vector2> Waypoints { get; } = new();

    public abstract IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = null);
    
    public Vector2 GetNextPathPosition(Vector2? currentPosition = null)
    {
        if (Waypoints.Count == 0)
        {
            return currentPosition ?? Vector2.Zero;
        }
        
        // if no current position is provided, then dequeue the next waypoint instead of just looking at peek
        var currentWaypoint = currentPosition.HasValue ? Waypoints.Peek() : Waypoints.Dequeue();

        // Check if the entity is close enough to the current waypoint position
        if (currentPosition.HasValue && currentPosition.Value.DistanceTo(currentWaypoint) <= WaypointReachedThreshold)
        {
            // If close enough, return the next new waypoint position and dequeue
            return Waypoints.Dequeue();
        }
        
        return currentWaypoint;
    }

    public int GetPathLength()
    {
        return Waypoints.Count;
    }

    /// <summary>
    /// Constructs the collection of vectors path from the destination node to the start node.
    /// Also adds the path to the waypoints queue.
    /// </summary>
    /// <param name="nodesPath">
    /// The dictionary that contains the path from the start node to the destination node.
    /// </param>
    /// <param name="destination">The destination node.</param>
    /// <param name="options">The custom options for pathfinding.</param>
    /// <returns>
    /// A collection of vectors that represent the path from the start to the destination.
    /// </returns>
    protected IEnumerable<Vector2> ConstructPath(IReadOnlyDictionary<Node, Node> nodesPath, Node destination, PathOptions options)
    {
        var (pathNodes, pathLength) = ConstructNodesPath(nodesPath, destination);
        var paths = new List<Vector2>(pathLength);
        
        foreach (var pathNode in pathNodes)
        {
            var path = options.ConvertToWorldPosition
                ? Grid.GetWorldPositionFromNode(pathNode, options.CellAlignment)
                : pathNode.Position;
            
            paths.Add(path);
            Waypoints.Enqueue(path);
        }
            
        return paths;
    }
    
    /// <summary>
    /// Constructs the path from the destination node to the start node.
    /// </summary>
    /// <param name="nodesPath">
    /// The dictionary that contains the path from the start node to the destination node.
    /// </param>
    /// <param name="destination">The destination node.</param>
    /// <returns>
    /// A tuple that contains the sequence of nodes from the start to the destination and the length of the path.
    /// </returns>
    private static (IEnumerable<Node>, int) ConstructNodesPath(IReadOnlyDictionary<Node, Node> nodesPath, Node destination)
    {
        var sequence = new Stack<Node>();
        var step = destination;

        while (nodesPath.ContainsKey(step))
        {
            sequence.Push(step);
            step = nodesPath[step];
        }
        
        sequence.Push(step);  // Push the start position
        return (sequence, sequence.Count);
    }
}
