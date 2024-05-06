using Godot;
using Maze.Scripts.Entities;

namespace Maze.Scripts.Scenes;

public partial class GameScene : BaseScene
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

	public override void _ExitTree()
	{
		GameManager.Instance.ScoreChanged -= UpdateScoreLabel;
	}

	#region Event Handlers

	private void UpdateScoreLabel(int score)
	{
		if (ScoreLabel is null)
		{
			return;
		}
		
		ScoreLabel.Text = $"Score: {score}";
	}

	#endregion
}
