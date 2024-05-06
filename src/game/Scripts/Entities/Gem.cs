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
		
		BodyEntered += OnBodyEntered;
	}

	public int GetValue()
	{
		return Type switch
		{
			GemType.Gold => 10,
			GemType.Diamond => 20,
			_ => 0
		};
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player)
		{
			player.CollectGem(GetValue());
			QueueFree();
		}
	}
}
