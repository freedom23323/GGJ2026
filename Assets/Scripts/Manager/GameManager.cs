using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, Playing, Paused, GameOver, Victory }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        UpdateState(GameState.MainMenu);
    }

    private void Update()
    {
        // 按 Esc 键切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Playing || CurrentState == GameState.Paused)
            {
                TogglePause();
            }
        }
    }
    public void UpdateState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0f;
                ResetScene();
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            case GameState.Victory:
                Time.timeScale = 0f;
                break;
        }

        // 通知 UIManager 更新界面
        UIManager.Instance.OnGameStateChanged(newState);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing) UpdateState(GameState.Paused);
        else if (CurrentState == GameState.Paused) UpdateState(GameState.Playing);
    }

    public void TriggerVictory()
    {
        UpdateState(GameState.Victory);
    }
    public void ReloadCompleteScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private List<ConsumableItem> allConsumables = new List<ConsumableItem>();

    public void RegisterConsumable(ConsumableItem item)
    {
        if (!allConsumables.Contains(item))
            allConsumables.Add(item);
    }

    public void ResetScene()
    {
        ScoreManager.Instance.ResetScore();
        MapManager.Instance.ResetMap();
        foreach (var item in allConsumables)
        {
            if (item != null) item.ResetItem();
        }

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Reset();
        }
    }
}