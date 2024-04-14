using Godot;
using Maze.Scripts.Constants;

namespace Maze.Scripts.GameObjects;

public partial class Player : CharacterBody2D
{
	private AnimatedSprite2D? _animatedSprite;
	
	[Export]
	public int Speed { get; set; } = 200;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		SetVelocity();
		UpdateAnimation();
		MoveAndSlide();
	}

	private void SetVelocity()
	{
		var inputDirection = Input.GetVector(InputActions.MoveLeft, InputActions.MoveRight, InputActions.MoveUp, InputActions.MoveDown);
		Velocity = inputDirection * Speed;
	}
	
	private void UpdateAnimation()
	{
		if (_animatedSprite is null)
		{
			return;
		}
		
		var velocity = Velocity;
		
		// Check if the player is moving and update the animation accordingly
		if (velocity.Length() > 0)
		{
			// Diagonal movements
			if (velocity is { X: > 0, Y: < 0 }) // Moving northeast
			{
				_animatedSprite.Animation = CharacterAnimations.WalkEast;
			}
			else if (velocity is { X: < 0, Y: < 0 }) // Moving northwest
			{
				_animatedSprite.Animation = CharacterAnimations.WalkWest;
			}
			else if (velocity is { X: > 0, Y: > 0 }) // Moving southeast
			{
				_animatedSprite.Animation = CharacterAnimations.WalkEast;
			}
			else if (velocity is { X: < 0, Y: > 0 }) // Moving southwest
			{
				_animatedSprite.Animation = CharacterAnimations.WalkWest;
			}
			
			// Straight movements
			else if (velocity.X > 0)
			{
				_animatedSprite.Animation = CharacterAnimations.WalkEast;
			}
			else if (velocity.X < 0)
			{
				_animatedSprite.Animation = CharacterAnimations.WalkWest;
			}
			else if (velocity.Y > 0)
			{
				_animatedSprite.Animation = CharacterAnimations.WalkSouth;
			}
			else if (velocity.Y < 0)
			{
				_animatedSprite.Animation = CharacterAnimations.WalkNorth;
			}
		}
		else
		{
			var animation = _animatedSprite.Animation.ToString();
			
			// Update to idle animations based on the last direction
			if (animation.Contains("east"))
			{
				_animatedSprite.Animation = CharacterAnimations.IdleEast;
			}
			else if (animation.Contains("west"))
			{
				_animatedSprite.Animation = CharacterAnimations.IdleWest;
			}
			else if (animation.Contains("south"))
			{
				_animatedSprite.Animation = CharacterAnimations.IdleSouth;
			}
			else if (animation.Contains("north"))
			{
				_animatedSprite.Animation = CharacterAnimations.IdleNorth;
			}
		}
		
		if (!_animatedSprite.IsPlaying())
		{
			_animatedSprite.Play();
		}
	}
}
