using System;
using Godot;
using Maze.Scripts.Constants;

namespace Maze.Scripts.UI;

public partial class MainMenu : Control
{
	#region Parameters

	[Export]
	public Button? StartButton { get; set; }
	
	[Export]
	public Button? TestAStarButton { get; set; }
	
	[Export]
	public Button? TestDijkstraButton { get; set; }
	
	[Export]
	public Button? TestBfsButton { get; set; }
	
	[Export]
	public Button? ExitButton { get; set; }

	#endregion
	
	
	public override void _Ready()
	{
		GameManager.Instance.ResetScore();
		AddEventHandler(StartButton, () => ChangeScene(ScenePaths.Game));
		AddEventHandler(TestAStarButton, () => ChangeToTestScene(ScenePaths.TestAStar));
		AddEventHandler(TestDijkstraButton, () => ChangeToTestScene(ScenePaths.TestDijkstra));
		AddEventHandler(TestBfsButton, () => ChangeToTestScene(ScenePaths.TestBfs));
		AddEventHandler(ExitButton, OnExitButtonPressed);
	}

	private void AddEventHandler(Button? button, Action action)
	{
		if (button is not null)
		{
			button.Pressed += action;
		}
	}

	private void ChangeScene(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}
	
	private void ChangeToTestScene(string scenePath)
	{
		GameManager.Instance.EnableDebug();
		GetTree().ChangeSceneToFile(scenePath);
	}
	
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
