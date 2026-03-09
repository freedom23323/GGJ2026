using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI 文本引用")]
    public TextMeshProUGUI scoreText;

    [Header("UI 面板引用")]
    public GameObject mainMenuPanel;
    public GameObject SettingPanel;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel; // 游戏进行时的血条、得分等

    public GameObject dialogueBox; // 对话框 UI 物体
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f; // 每个字的间隔时间
    public bool isDialogueActive = false;

    private Coroutine typingCoroutine;

    [Header("胜利面板")]
    public GameObject victoryPanel;
    public void ShowDialogue(string content)
    {
        isDialogueActive = true;
        dialogueBox.SetActive(true);

        // 如果正在打字，先停止之前的
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(content));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            // 每打一个字播放一个轻微的音效点（可选）
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void HideDialogue() => dialogueBox.SetActive(false);

    [Header("等级显示")]
    public TMPro.TextMeshProUGUI levelText;

    public void UpdateLevelDisplay(int levelNumber)
    {
        if (levelText != null)
        {
            levelText.text = "等级 " + levelNumber.ToString("00");
        }
    }



    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        // 订阅事件：当分数改变时执行 UpdateScoreText 方法
        ScoreManager.OnScoreChanged += UpdateScoreText;
    }

    private void OnDisable()
    {
        // 取消订阅（防止内存泄漏的好习惯）
        ScoreManager.OnScoreChanged -= UpdateScoreText;
    }

    // 真正的 UI 更新逻辑
    private void UpdateScoreText(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString();
        }
    }

    // 根据游戏状态切换显示的面板
    public void OnGameStateChanged(GameState state)
    {
        mainMenuPanel?.SetActive(state == GameState.MainMenu);
        pausePanel?.SetActive(state == GameState.Paused);
        gameOverPanel?.SetActive(state == GameState.GameOver);
        hudPanel?.SetActive(state == GameState.Playing);
        victoryPanel?.SetActive(state == GameState.Victory);
    }

    // 按钮点击事件
    public void StartGame() => GameManager.Instance.UpdateState(GameState.Playing);
    public void ResumeGame() => GameManager.Instance.UpdateState(GameState.Playing);

    public void ReturnMainMenu() => GameManager.Instance.UpdateState(GameState.MainMenu);

    public void QuitGame() => Application.Quit();

    public void OpenSettings() => SettingPanel.SetActive(true);
    public void CloseSettings() => SettingPanel.SetActive(false);
}