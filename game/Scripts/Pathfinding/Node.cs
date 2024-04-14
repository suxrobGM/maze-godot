using Godot;

namespace Maze.Scripts.Pathfinding;

public class Node
{
    public Vector2 Position { get; set; }
    public int Cost { get; set; }
    public Node? Parent { get; set; } 
    
    public Node(Vector2 position, int cost = 1) {
        Position = position;
        Cost = cost;
    }
}
