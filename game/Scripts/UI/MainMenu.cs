using Godot;

namespace Maze.Scripts.UI;

public partial class MainMenu : Control
{
	[Export]
	public Button StartButton { get; set; }
	
	[Export]
	public Button ExitButton { get; set; }
	
	public override void _Ready()
	{
		StartButton.Pressed += OnStartButtonPressed;
		ExitButton.Pressed += OnExitButtonPressed;
	}
	
	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Maze.tscn");
	}
	
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
