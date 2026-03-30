using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting.FullSerializer;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI 文本引用")]
    public TextMeshProUGUI scoreText;
    [Header("等级显示")]
    public TMPro.TextMeshProUGUI levelText;
    [Header("钥匙")]
    public TextMeshProUGUI KeyText;

    [Header("UI 面板引用")]
    public GameObject mainMenuPanel;
    public GameObject SettingPanel;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel; // 游戏进行时的血条、得分等

    public GameObject dialogueBox; // 对话框 UI 物体
    public TextMeshProUGUI dialogueText;

    public GameObject[] dialogueBoxs; // 预设的对话框列表，角色专属对话框配置在 DialogBoxConfig 中

    public float typingSpeed = 0.05f; // 每个字的间隔时间
    public bool isDialogueActive = false;
    public GameObject StoryPanel;

    private Coroutine typingCoroutine;

    [Header("胜利面板")]
    public GameObject victoryPanel;
    public void ShowDialogue(GameObject speaker,string content)
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
        // 获取角色身上的对话框配置
        DialogBoxConfig config = speaker.GetComponent<DialogBoxConfig>();
        if (config == null)
        {
            Debug.LogWarning($"角色{speaker.name}未配置专属对话框");
            return;
        }
        UpdateDialogueBox(config);
        ShowSentence(content);
    }

    public void ShowDialogue(int index, string content)
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
        // 获取角色身上的对话框配置
        UpdateDialogueBox(index);
        ShowSentence(content);
    }
    private void ShowSentence(string content)
    {
        HideHudPanel();
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
    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        ShowHudPanel();
    }

    public void HideHudPanel() => hudPanel.SetActive(false);

    public void ShowHudPanel() => hudPanel.SetActive(true);

    public void UpdateLevelDisplay(int levelNumber)
    {
        if (levelText != null)
        {
            levelText.text = "等级 " + levelNumber.ToString("00");
        }
    }

    public void UpdateKeyDisplay(int keyCount)
    {
        if (KeyText != null)
        {
            KeyText.text = "key: " + keyCount.ToString();
        }
    }

    private void UpdateDialogueBox(DialogBoxConfig config)
    {
        dialogueBox = config.dialogBox;
        dialogueText= dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void UpdateDialogueBox(int index)
    {
        dialogueBox = dialogueBoxs[index];
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
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
    public void StartGame()
    {
        GameManager.Instance.UpdateState(GameState.Playing);
        StoryPanel.SetActive(true);
    }
    public void ResumeGame() => GameManager.Instance.UpdateState(GameState.Playing);

    public void ReturnMainMenu() => GameManager.Instance.UpdateState(GameState.MainMenu);

    public void QuitGame() => Application.Quit();

    public void OpenSettings() => SettingPanel.SetActive(true);
    public void CloseSettings() => SettingPanel.SetActive(false);
}