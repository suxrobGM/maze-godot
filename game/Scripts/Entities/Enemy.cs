using Godot;
using Maze.Scripts.Pathfinding;

namespace Maze.Scripts.Entities;

public partial class Enemy : CharacterBody2D
{
	private AnimatedSprite2D _redAnimatedSprite = default!;
	private AnimatedSprite2D _yellowAnimatedSprite = default!;
	private NavigationAgent2D _navigationAgent = default!;
	private Timer _timer = default!;
	private PathDebugger _pathDebugger = default!;
	private PathOptions _pathOptions = default!;
	private IPathfinder _pathfinder = default!;
	private AcceptDialog? _messageBox;
	private MazeTileMap? _maze;
	private Player? _player;
	private ulong _pathStartTime;
	private bool _alreadyCollidedWithPlayer;
	
	#region Parameters

	[Export]
	public EnemyType Type { get; set; }
	
	[Export]
	public bool CanMove { get; set; } = true;
	
	[Export]
	public int Speed { get; set; } = 150;
	
	[Export, ExportGroup("Pathfinder")]
	public PathfindingAlgorithmType PathfindingAlgorithm { get; set; }
	
	[Export, ExportGroup("Pathfinder")]
	public Alignment PathCellAlignment { get; set; }
	
	[Export, ExportGroup("Pathfinder")]
	public Color DebugPathColor { get; set; } = Colors.Red;
	
	[Export, ExportGroup("Pathfinder")]
	public bool MeasurePerformance { get; set; }
	
	#endregion
	

	public override void _Ready()
	{
		_messageBox = GetTree().CurrentScene.GetNode<AcceptDialog>("UI/MessageBox");
		_player = GetTree().CurrentScene.GetNode<Player>("Player");
		_maze = GetTree().CurrentScene.GetNode<MazeTileMap>("MazeTileMap");
		
		if (_player is null)
		{
			GD.PrintErr("Player is not found from scene tree");
		}

		if (_maze is null)
		{
			GD.PrintErr("MazeTileMap is not found from scene tree");
		}
		
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
		if (_maze is null)
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
			PathfindingAlgorithmType.AStar => new AStarPathfinder(_maze.GetGrid()),
			PathfindingAlgorithmType.Dijkstra => new DijkstraPathfinder(_maze.GetGrid()),
			PathfindingAlgorithmType.Bfs => new BfsPathfinder(_maze.GetGrid()),
			_ => _pathfinder
		};
		
		_navigationAgent = GetNode<NavigationAgent2D>("Navigation/NavigationAgent2D");
		_timer = GetNode<Timer>("Navigation/Timer");
		_pathDebugger = GetNode<PathDebugger>("Navigation/PathDebugger");
		_pathDebugger.DefaultColor = DebugPathColor;
		_navigationAgent.DebugUseCustom = true;
		_navigationAgent.DebugPathCustomColor = DebugPathColor;
		TogglePathDebugger(GameManager.Instance.IsDebugEnabled);
		
		_timer.Timeout += UpdateDestinationPath;
		_timer.Start();
		_pathStartTime = Time.GetTicksMsec();
	}

	private void MoveTowardsPlayer()
	{
		if (!CanMove)
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

			if (direction == Vector2.Zero)
			{
				direction = Position;
			}
		}
		
		Velocity = ToLocal(direction).Normalized() * Speed;
		MoveAndSlide();
		HandleCollisionWithPlayer();
	}

	private void UpdateDestinationPath()
	{
		if (PathfindingAlgorithm is PathfindingAlgorithmType.AStar)
		{
			_navigationAgent.TargetPosition = _player?.Position ?? Vector2.Zero;
		}
		else
		{
			var paths = _pathfinder.FindPath(Position, _player?.Position ?? Vector2.Zero, _pathOptions);

			if (GameManager.Instance.IsDebugEnabled)
			{
				_pathDebugger.DrawPath(paths);
			}
		}
	}
	
	private void TogglePathDebugger(bool isDebugMode)
	{
		_pathDebugger.Visible = isDebugMode;
		_navigationAgent.DebugEnabled = isDebugMode;
	}

	private void HandleCollisionWithPlayer()
	{
		if (_alreadyCollidedWithPlayer)
		{
			return;
		}	
		
		var lastCollision = GetLastSlideCollision();

		if (lastCollision?.GetCollider() is not Player)
		{
			return;
		}
		
		if (MeasurePerformance && _messageBox is not null)
		{
			_alreadyCollidedWithPlayer = true;
			_messageBox.Title = "Performance";
			_messageBox.DialogText = $"Pathfinding '{PathfindingAlgorithm}' performance: {Time.GetTicksMsec() - _pathStartTime} ms";
			_messageBox.Show();
		}

		_pathStartTime = Time.GetTicksMsec(); // Reset the path start time
	}
}
