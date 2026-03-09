using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // 定义事件：当分数改变时，发送当前的整数分
    public static event Action<int> OnScoreChanged;

    private int currentScore = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        // 触发事件：通知所有“订阅者”（比如 UI）分数变了
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
        OnScoreChanged?.Invoke(currentScore);
    }
    public int GetCurrentScore() => currentScore;
}