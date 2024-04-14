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
	public TileMap? TileMap { get; set; }
	
	[Export]
	public Player? Player { get; set; }
	
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
		
		// UpdateDestinationPath();
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveTowardsPlayer();
	}
	
	private void InitPathfinder()
	{
		_pathfinder = PathfindingAlgorithm switch
		{
			PathfindingAlgorithmType.AStar => new AStarPathfinder(),
			PathfindingAlgorithmType.Dijkstra => new DijkstraPathfinder(),
			PathfindingAlgorithmType.Bfs => new BfsPathfinder(),
			_ => _pathfinder
		};
	}

	private void MoveTowardsPlayer()
	{
		if (!CanMove || _navigationAgent is null)
		{
			return;
		}
		
		var direction = ToLocal(_navigationAgent.GetNextPathPosition()).Normalized();
		Velocity = direction * Speed;
		MoveAndSlide();
	}

	private void UpdateDestinationPath()
	{
		if (_navigationAgent is null)
		{
			return;
		}
		
		_navigationAgent.TargetPosition = Player?.GlobalPosition ?? Vector2.Zero;
	}
}
