using System;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class Node : IEquatable<Node>
{
    public Node(Vector2 position, int cost) 
    {
        Position = position;
        Cost = cost;
    }
    
    /// <summary>
    /// The position of the node in the grid. It's a tile position.
    /// </summary>
    public Vector2 Position { get; } // x, and y coordinates in the grid
    
    /// <summary>
    /// The cost of the node.
    /// </summary>
    public int Cost { get; set; }

    public bool Equals(Node? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        
        return ReferenceEquals(this, other) || Position.Equals(other.Position);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        return obj.GetType() == GetType() && Equals((Node)obj);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Position} - {Cost}";
    }
}
