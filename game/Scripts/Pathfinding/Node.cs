using Godot;

namespace Maze.Scripts.Pathfinding;

public class Node
{
    public Node(Vector2 gridPosition, Vector2 worldPosition, int cost) {
        GridPosition = gridPosition;
        WorldPosition = worldPosition;
        Cost = cost;
    }
    
    /// <summary>
    /// The position of the node in the grid. It's a tile position.
    /// </summary>
    public Vector2 GridPosition { get; }
    
    /// <summary>
    /// The position of the node in the world. It's a global position.
    /// Assuming anchor point is in the top-left corner.
    /// </summary>
    public Vector2 WorldPosition { get; }
    
    /// <summary>
    /// The cost of the node.
    /// </summary>
    public int Cost { get; set; }
    
    public override string ToString() {
        return $"Grid: {GridPosition}, World: {WorldPosition}, Cost: {Cost}";
    }
}
