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
    /// <returns>
    /// A collection of positions that represent the path from the start to the destination.
    /// </returns>
    IEnumerable<Vector2> FindPath(Vector2 start, Vector2 destination);
    
    /// <summary>
    /// Retrieves the next position in the path queue and optionally converts it from grid coordinates to world coordinates.
    /// </summary>
    /// <param name="convertToWorldPosition">If true, converts the grid coordinates to world coordinates based on the cell size of the grid. If false, returns the raw grid coordinates.</param>
    /// <param name="centerToCell">If true, returns the position centered in the middle of the cell. If false, returns the position at the top-left corner of the cell.</param>
    /// <returns>The next position in the path as a <see cref="Vector2"/>. If no positions are left in the queue, returns <see cref="Vector2.Zero"/>.</returns>
    Vector2 GetNextPathPosition(bool convertToWorldPosition = true, bool centerToCell = false);
    
    /// <summary>
    /// Gets the length of the path queue.
    /// </summary>
    int GetPathLength();
}
