using Godot;

namespace Maze.Scripts.Pathfinding;

public class Node
{
    public Vector2 Position { get; set; }
    public int Cost { get; set; }
    
    public Node(Vector2 position, int cost) {
        Position = position;
        Cost = cost;
    }
    
    public override string ToString() {
        return $"Node(Position: {Position}, Cost: {Cost})";
    }
}
