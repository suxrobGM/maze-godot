using System;

namespace Maze.Scripts;

public class GameManager
{
    private int _score;
    
    private GameManager()
    {
    }
    
    public static GameManager Instance { get; } = new();
    
    public event EventHandler<int>? ScoreChanged;
    
    public void AddScore(int score)
    {
        _score += score;
        ScoreChanged?.Invoke(this, _score);
    }
    
    public void ResetScore()
    {
        _score = 0;
        ScoreChanged?.Invoke(this, _score);
    }
}
