using System.Linq;
using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.GameObjects;

public partial class Monster : CharacterBody2D
{
	private AnimatedSprite2D? _animatedSprite;
	private NavigationAgent2D? _navigationAgent;
	private Timer? _timer;
	private IPathfinder _pathfinder = default!;
	
	#region Parameters

	[Export]
	public MonsterType Type { get; set; }
	
	[Export]
	public bool CanMove { get; set; } = true;
	
	[Export]
	public int Speed { get; set; } = 150;
	
	[Export]
	public PathfindingAlgorithmType PathfindingAlgorithm { get; set; } = PathfindingAlgorithmType.AStar;
	
	[Export]
	public Maze? Maze { get; set; }
	
	[Export]
	public Player? Player { get; set; }
	
	[Export]
	public PathDebugger? PathDebugger { get; set; }
	
	#endregion
	

	public override void _Ready()
	{
		InitPathfinder();
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_navigationAgent = GetNode<NavigationAgent2D>("Navigation/NavigationAgent2D");
		_timer = GetNode<Timer>("Navigation/Timer");
		
		if (_timer is not null)
		{
			_timer.Timeout += UpdateDestinationPath;
			_timer.Start();
		}
		
		UpdateDestinationPath();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveTowardsPlayer();
	}
	
	private void InitPathfinder()
	{
		if (Maze is null)
		{
			return;
		}
		
		_pathfinder = PathfindingAlgorithm switch
		{
			PathfindingAlgorithmType.AStar => new AStarPathfinder(),
			PathfindingAlgorithmType.Dijkstra => new DijkstraPathfinder(Maze.GetGrid()),
			PathfindingAlgorithmType.Bfs => new BfsPathfinder(Maze.GetGrid()),
			_ => _pathfinder
		};
	}

	private void MoveTowardsPlayer()
	{
		if (!CanMove || _navigationAgent is null)
		{
			return;
		}
		
		//var direction =  ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		var nextNodePath = _pathfinder.GetNextPathPosition();
		var direction = new Vector2(nextNodePath.X * Maze!.GetTileWidth(), nextNodePath.Y * Maze.GetTileHeight());
		
		GD.Print($"Direction: {ToLocal(direction)}");
		Velocity = ToLocal(direction).Normalized() * Speed;
		MoveAndSlide();
	}

	private void UpdateDestinationPath()
	{
		if (_navigationAgent is null)
		{
			return;
		}
		
		//_navigationAgent.TargetPosition = Player?.GlobalPosition ?? Vector2.Zero;
		GD.Print($"Enemy: {Position}, Player: {Player?.Position}");
		var paths = _pathfinder.FindPath(Position, Player?.Position ?? Vector2.Zero)
			.Select(path => new Vector2(path.X * Maze!.GetTileWidth(), path.Y * Maze.GetTileHeight()))
			.ToList();
		
		PathDebugger?.DrawPath(paths);
	}
}
