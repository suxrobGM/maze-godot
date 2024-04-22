namespace Maze.Scripts.Pathfinding;

public record PathOptions
{
    /// <summary>
    /// Convert the path from grid tile position to world position.
    /// </summary>
    public bool ConvertToWorldPosition { get; set; }
    
    /// <summary>
    /// Specify the alignment value when converting the path to world position.
    /// Top-left is the default alignment.
    /// </summary>
    public Alignment CellAlignment { get; set; }
}
