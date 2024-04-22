using Godot;
using Maze.Scripts.Constants;

namespace Maze.Scripts.Scenes;

public abstract partial class BaseScene : Node2D
{
    public override void _Input(InputEvent input)
    {
        if (input.IsActionPressed(InputActions.PauseMenu))
        {
            GetTree().ChangeSceneToFile(ScenePaths.MainMenu);
        }
        
        if (input.IsActionPressed(InputActions.ToggleDebug))
        {
            GameManager.Instance.ToggleDebug();
        }
    }
}
