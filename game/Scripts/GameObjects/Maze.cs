using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.GameObjects;

public partial class Maze : TileMap
{
	private Grid _grid = default!;
	
	public override void _Ready()
	{
		_grid = GridFactory.CreateGridFromTileMap(this);
	}
}
