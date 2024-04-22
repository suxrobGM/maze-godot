using System;
using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.Scenes;

public partial class MazeScene : Node2D
{
	private Grid _grid = default!;

	#region Parameters

	[Export]
	public Label? ScoreLabel { get; set; }
	
	[Export]
	public TileMap? TileMap { get; set; }

	#endregion
	
	public override void _Ready()
	{
		if (TileMap is null)
		{
			throw new InvalidOperationException("TileMap is not set");
		}
		
		_grid = GridFactory.CreateGridFromTileMap(TileMap);
		GameManager.Instance.ScoreChanged += UpdateScoreLabel;
	}

	public Grid GetGrid() => _grid;
	public int GetTileSize() => _grid.CellSize;

	#region Event Handlers

	private void UpdateScoreLabel(object? sender, int score)
	{
		if (ScoreLabel is null)
		{
			return;
		}
		
		ScoreLabel.Text = $"Score: {score}";
	}

	#endregion
}
