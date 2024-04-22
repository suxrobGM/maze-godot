using Godot;
using Maze.Scripts.Entities;

namespace Maze.Scripts.Scenes;

public partial class GameScene : Node2D
{
	#region Parameters

	[Export]
	public Label? ScoreLabel { get; set; }
	
	[Export]
	public MazeTileMap? Maze { get; set; }

	#endregion
	
	public override void _Ready()
	{
		GameManager.Instance.ScoreChanged += UpdateScoreLabel;
	}

	public override void _Input(InputEvent input)
	{
		if (input.IsAction(InputActions.PauseMenu))
		{
			GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
		}
	}

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
