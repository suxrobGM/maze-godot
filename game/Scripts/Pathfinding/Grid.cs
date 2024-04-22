using System.Collections.Generic;
using Godot;

namespace Maze.Scripts.Pathfinding;

public class Grid
{
    public Grid(int width, int height, int cellSize = 32)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        Nodes = new Node[width, height];
        InitEmptyNodes();
    }
    
    public Node[,] Nodes { get; }
    public int Width { get; }
    public int Height { get; }
    public int CellSize { get; }
    public int Size => Width * Height;
    
    public Node this[int x, int y] => Nodes[x, y];
    
    public Node GetNodeFromWorldPosition(Vector2 worldPosition)
    {
        var x = Mathf.FloorToInt(worldPosition.X / CellSize);
        var y = Mathf.FloorToInt(worldPosition.Y / CellSize);
        return this[x, y];
    }
    
    public Vector2 GetWorldPositionFromNode(Node node, Alignment alignment = Alignment.TopLeft)
    {
        var worldPosition = new Vector2();
        
        if (alignment is Alignment.Center)
        {
            worldPosition.X = node.Position.X * CellSize + CellSize / 2;
            worldPosition.Y = node.Position.Y * CellSize + CellSize / 2;
        }
        else
        {
            worldPosition.X = node.Position.X * CellSize;
            worldPosition.Y = node.Position.Y * CellSize;
        }
        
        return worldPosition;
    }

    /// <summary>
    /// Get the neighbors of a node.
    /// </summary>
    /// <param name="node">The node to get the neighbors from.</param>
    /// <returns>The neighbors of the node.</returns>
    public IEnumerable<Node> GetNeighbors(Node node)
    {
        var directions = new List<Vector2>
        {
            new(1, 0),  // Right
            new(-1, 0), // Left
            new(0, 1),  // Down
            new(0, -1)  // Up
        };

        foreach (var dir in directions)
        {
            var next = new Vector2(node.Position.X + dir.X, node.Position.Y + dir.Y);
            if (next.X >= 0 && next.X < Width && next.Y >= 0 && next.Y < Height)
            {
                yield return this[(int)next.X, (int)next.Y];
            }
        }
    }
    
    private void InitEmptyNodes()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                Nodes[x, y] = new Node(new Vector2(x, y), 0);
            }
        }
    }
}
