using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public interface IPathfinder
{
    /// <summary>
    /// Generates paths from the start position to the destination position using the implemented pathfinding algorithm.
    /// </summary>
    /// <param name="start">The starting position</param>
    /// <param name="destination">The destination position</param>
    /// <param name="options">Custom options for pathfinding</param>
    /// <returns>
    /// A collection of positions that represent the path from the start to the destination.
    /// </returns>
    IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = null);
    
    /// <summary>
    /// Retrieves the next position in the path queue.
    /// If the current position is provided, it checks if the entity is close enough to the current waypoint.
    /// </summary>
    /// <param name="currentPosition">
    /// The optional current position of the entity. It checks if the entity is close enough to the current waypoint. If so, it returns the next waypoint.
    /// Otherwise, it returns the current waypoint.
    /// </param>
    /// <returns>
    /// The next position in the path as a <see cref="Vector2"/>.
    /// If no positions are left in the queue, and not specified the currentPosition then returns <see cref="Vector2.Zero"/>.
    /// If specified the currentPosition and no waypoints left in the queue, then returns the currentPosition.
    /// </returns>
    Vector2 GetNextPathPosition(Vector2? currentPosition = null);
    
    /// <summary>
    /// Gets the length of the path queue.
    /// </summary>
    int GetPathLength();
}
