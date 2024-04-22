using System;

namespace Maze.Scripts;

public sealed class GameManager
{
    private int _score;
    private bool _isDebugEnabled;
    
    private GameManager()
    {
    }
    
    public static GameManager Instance { get; } = new();
    
    #region Events

    public event Action<int>? ScoreChanged;
    public event Action<bool>? DebugModeChanged; 

    #endregion

    #region Properties

    public bool IsDebugEnabled
    {
        get => _isDebugEnabled;
        private set
        {
            _isDebugEnabled = value;
            DebugModeChanged?.Invoke(_isDebugEnabled);
        }
    }

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
        IsDebugEnabled = !IsDebugEnabled;
    }
    
    public void EnableDebug()
    {
        IsDebugEnabled = true;
    }
}
