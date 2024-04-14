using Godot;

namespace Maze.Scripts.Pathfinding;

public class Grid
{
    private readonly Node[,] _nodes;
    
    public Grid(int width, int height, int cellSize = 32)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        _nodes = new Node[width, height];
    }
    
    public int Width { get; }
    public int Height { get; }
    public int CellSize { get; }
    public int Size => Width * Height;
    
    public Node this[int x, int y]
    {
        get => _nodes[x, y];
        set => _nodes[x, y] = value;
    }
    
    public Node this[Vector2 position]
    {
        get => GetNodeFromWorldPosition(position);
        set
        {
            var (x, y) = ConvertWorldToGridPosition(position, CellSize);
            _nodes[x, y] = value;
        }
    }
    
    private Node GetNodeFromWorldPosition(Vector2 worldPosition)
    {
        var x = Mathf.FloorToInt(worldPosition.X / CellSize);
        var y = Mathf.FloorToInt(worldPosition.Y / CellSize);
        return _nodes[x, y];
    }
    
    private (int, int) ConvertWorldToGridPosition(Vector2 worldPosition, int cellSize)
    {
        var x = Mathf.FloorToInt(worldPosition.X / cellSize);
        var y = Mathf.FloorToInt(worldPosition.Y / cellSize);
        return (x, y);
    }
}
