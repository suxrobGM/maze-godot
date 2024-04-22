using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.Entities;

public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D? _redAnimatedSprite;
	private AnimatedSprite2D? _yellowAnimatedSprite;
	private NavigationAgent2D? _navigationAgent;
	private Timer? _timer;
	private PathDebugger? _pathDebugger;
	private PathOptions _pathOptions = default!;
	private IPathfinder _pathfinder = default!;
	
	#region Parameters

	[Export]
	public EnemyType Type { get; set; }
	
	[Export]
	public bool CanMove { get; set; } = true;
	
	[Export]
	public int Speed { get; set; } = 150;
	
	[Export]
	public MazeTileMap? Maze { get; set; }
	
	[Export]
	public Player? Player { get; set; }
	
	[Export, ExportGroup("Pathfinder")]
	public PathfindingAlgorithmType PathfindingAlgorithm { get; set; }
	
	[Export, ExportGroup("Pathfinder")]
	public Alignment PathCellAlignment { get; set; }
	
	[Export, ExportGroup("Pathfinder")]
	public Color DebugPathColor { get; set; } = Colors.Red;
	
	#endregion
	

	public override void _Ready()
	{
		InitSprite();
		InitPathfinder();
		GameManager.Instance.DebugModeChanged += TogglePathDebugger;
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveTowardsPlayer();
	}

	public override void _ExitTree()
	{
		GameManager.Instance.DebugModeChanged -= TogglePathDebugger;
	}

	private void InitSprite()
	{
		_redAnimatedSprite = GetNode<AnimatedSprite2D>("RedAnimatedSprite");
		_yellowAnimatedSprite = GetNode<AnimatedSprite2D>("YellowAnimatedSprite");

		switch (Type)
		{
			case EnemyType.Red:
				_redAnimatedSprite.Visible = true;
				_yellowAnimatedSprite.Visible = false;
				break;
			case EnemyType.Yellow:
				_redAnimatedSprite.Visible = false;
				_yellowAnimatedSprite.Visible = true;
				break;
		}
	}
	
	private void InitPathfinder()
	{
		if (Maze is null)
		{
			return;
		}
		
		_pathOptions = new PathOptions
		{
			ConvertToWorldPosition = true,
			CellAlignment = PathCellAlignment
		};
		
		_pathfinder = PathfindingAlgorithm switch
		{
			PathfindingAlgorithmType.AStar => new AStarPathfinder(Maze.GetGrid()),
			PathfindingAlgorithmType.Dijkstra => new DijkstraPathfinder(Maze.GetGrid()),
			PathfindingAlgorithmType.Bfs => new BfsPathfinder(Maze.GetGrid()),
			_ => _pathfinder
		};
		
		_navigationAgent = GetNode<NavigationAgent2D>("Navigation/NavigationAgent2D");
		_timer = GetNode<Timer>("Navigation/Timer");
		_pathDebugger = GetNode<PathDebugger>("Navigation/PathDebugger");
		_pathDebugger.DefaultColor = DebugPathColor;
		_navigationAgent.DebugUseCustom = true;
		_navigationAgent.DebugPathCustomColor = DebugPathColor;
		
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

	private void MoveTowardsPlayer()
	{
		if (!CanMove || _navigationAgent is null)
		{
			return;
		}

		Vector2 direction;

		if (PathfindingAlgorithm is PathfindingAlgorithmType.AStar)
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
		
		Velocity = ToLocal(direction).Normalized() * Speed;
		MoveAndSlide();
	}

	private void UpdateDestinationPath()
	{
		if (_navigationAgent is null)
		{
			return;
		}

		if (PathfindingAlgorithm is PathfindingAlgorithmType.AStar)
		{
			_navigationAgent.TargetPosition = Player?.Position ?? Vector2.Zero;
		}
		else
		{
			var paths = _pathfinder.FindPath(Position, Player?.Position ?? Vector2.Zero, _pathOptions);

			if (GameManager.Instance.IsDebugModeEnabled)
			{
				_pathDebugger?.DrawPath(paths);
			}
		}
	}
	
	private void TogglePathDebugger(bool isDebugMode)
	{
		if (_pathDebugger is not null)
		{
			_pathDebugger.Visible = isDebugMode;
		}

		if (_navigationAgent is not null)
		{
			_navigationAgent.DebugEnabled = isDebugMode;
		}
	}
}
