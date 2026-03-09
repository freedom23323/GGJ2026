using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("地图配置")]
    public List<GameObject> levels; // 在 Inspector 里按顺序拖入你的地图/等级物体
    private int currentLevelIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 初始时初始化第一关
        InitializeLevel(0);
    }

    // 初始化特定等级的地图
    public void InitializeLevel(int index)
    {
        if (index < 0 || index >= levels.Count) return;

        // 关闭所有地图
        foreach (var level in levels)
        {
            level.SetActive(false);
        }

        // 开启当前地图
        currentLevelIndex = index;
        levels[currentLevelIndex].SetActive(true);
        Debug.Log(currentLevelIndex);
        // 通知 UIManager 更新等级显示
        UIManager.Instance.UpdateLevelDisplay(currentLevelIndex + 1);
    }

    // 切换到下一关
    public void NextLevel()
    {
        InitializeLevel(currentLevelIndex + 1);
    }
    // 在 MapManager.cs 中添加
    public void PreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            InitializeLevel(currentLevelIndex - 1);
        }
    }
    // 在 MapManager.cs 中添加
    public int GetCurrentLevelIndex()
    {
        // levels 列表中的索引是从 0 开始的
        return currentLevelIndex;
    }
    public void ResetMap()
    {
        InitializeLevel(0); // 加载第一个等级
    }
}