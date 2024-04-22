using Godot;

namespace Maze.Scripts.Entities;

public partial class Gem : Area2D
{
	private AnimatedSprite2D? _animatedSprite;
	
	#region Parameters

	[Export]
	public GemType Type { get; set; }

	#endregion

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite?.Play();
	}
}
