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
    IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination, PathOptions? options = default);
    
    /// <summary>
    /// Retrieves the next position in the path queue.
    /// </summary>
    /// <returns>
    /// The next position in the path as a <see cref="Vector2"/>. If no positions are left in the queue, returns <see cref="Vector2.Zero"/>.
    /// </returns>
    Vector2 GetNextPathPosition();
    
    /// <summary>
    /// Gets the length of the path queue.
    /// </summary>
    int GetPathLength();
}
