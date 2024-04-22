using System;

namespace Maze.Scripts;

public sealed class GameManager
{
    private int _score;
    private bool _isDebugModeEnabled;
    
    private GameManager()
    {
    }
    
    public static GameManager Instance { get; } = new();

    #region Properties

    public bool IsDebugModeEnabled
    {
        get => _isDebugModeEnabled;
        private set
        {
            _isDebugModeEnabled = value;
            DebugModeChanged?.Invoke(_isDebugModeEnabled);
        }
    }

    #endregion

    #region Events

    public event Action<int>? ScoreChanged;
    public event Action<bool>? DebugModeChanged; 

    #endregion
    
    
    public void AddScore(int value)
    {
        _score += value;
        ScoreChanged?.Invoke(_score);
    }
    
    public void ResetScore()
    {
        _score = 0;
        ScoreChanged?.Invoke(_score);
    }
    
    public void ToggleDebug()
    {
        IsDebugModeEnabled = !IsDebugModeEnabled;
    }
}
