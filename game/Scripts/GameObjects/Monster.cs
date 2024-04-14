using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.GameObjects;

public partial class Monster : CharacterBody2D
{
	private AnimatedSprite2D? _animatedSprite;
	
	#region Parameters

	[Export]
	public MonsterType Type { get; set; }
	
	[Export]
	public bool CanMove { get; set; } = true;
	
	[Export]
	public PathfindingAlgorithmType PathfindingAlgorithm { get; set; } = PathfindingAlgorithmType.AStar;
	
	[Export]
	public TileMap? TileMap { get; set; }
	
	[Export]
	public Player? Player { get; set; }
	

	#endregion
	

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
}
