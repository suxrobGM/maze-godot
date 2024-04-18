using System.Linq;
using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.GameObjects;

public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D? _animatedSprite;
	private NavigationAgent2D? _navigationAgent;
	private Timer? _timer;
	private IPathfinder _pathfinder = default!;
	
	#region Parameters

	[Export]
	public EnemyType Type { get; set; }
	
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
			if (PathfindingAlgorithm is not PathfindingAlgorithmType.AStar)
			{
				_timer.WaitTime = 0.5;
			}
			
			_timer.Timeout += UpdateDestinationPath;
			_timer.Start();
		}
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
			PathfindingAlgorithmType.AStar => new AStarPathfinder(Maze.GetGrid()),
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

		Vector2 direction;

		if (PathfindingAlgorithm == PathfindingAlgorithmType.AStar)
		{
			direction = _navigationAgent.GetNextPathPosition();
		}
		else
		{
			if (_pathfinder.GetPathLength() == 0)
			{
				UpdateDestinationPath();
			}
			
			direction = _pathfinder.GetNextPathPosition();
		}
		
		//GD.Print($"Enemy: {ToLocal(direction)}, Player: {ToLocal(Player?.Position ?? Vector2.Zero)}, Node Position: {direction}");
		Velocity = ToLocal(direction).Normalized() * Speed;
		MoveAndSlide();
	}

	private void UpdateDestinationPath()
	{
		if (_navigationAgent is null)
		{
			return;
		}

		if (PathfindingAlgorithm == PathfindingAlgorithmType.AStar)
		{
			_navigationAgent.TargetPosition = Player?.GlobalPosition ?? Vector2.Zero;
		}
		else
		{
			var paths = _pathfinder.FindPath(Position, Player?.Position ?? Vector2.Zero)
				.Select(path => new Vector2(path.X * Maze!.GetTileSize(), path.Y * Maze.GetTileSize()))
				.ToList();
		
			PathDebugger?.DrawPath(paths);
		}
	}
}
