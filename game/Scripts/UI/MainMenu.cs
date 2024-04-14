using Godot;

namespace Maze.Scripts.UI;

public partial class MainMenu : Control
{
	[Export]
	public Button? StartButton { get; set; }
	
	[Export]
	public Button? ExitButton { get; set; }
	
	public override void _Ready()
	{
		if (StartButton is not null)
		{
			StartButton.Pressed += OnStartButtonPressed;
		}

		if (ExitButton is not null)
		{
			ExitButton.Pressed += OnExitButtonPressed;
		}
	}
	
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	}
	
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
