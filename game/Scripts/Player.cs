using System;
using Godot;

namespace Maze.Scripts;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;

	public override void _PhysicsProcess(double delta)
	{
		SetVelocity();
		MoveAndSlide();
	}

	private void SetVelocity()
	{
		var inputDirection = Input.GetVector(InputActions.Left, InputActions.Right, InputActions.Up, InputActions.Down);
		Velocity = inputDirection * Speed;
	}
}
