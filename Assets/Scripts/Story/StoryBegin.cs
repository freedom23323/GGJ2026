using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBegin : MonoBehaviour
{
    [Header("对话配置")]
    public Sprite[] dialogueSprites;
    public Image dialogueImage;
    public TextMeshProUGUI clickText;

    [Header("演出配置")]
    public RectTransform storyPanel;
    public RectTransform zoomTarget;

    [Header("演示")]
    public Image playerImage;

    [Header("动画参数")]
    public float zoomDuration = 1.5f;

    private int index = 0;
    private bool isPlaying = false;

    private Vector3 _initScale;
    private Vector3 _initPos;

    void Start()
    {
        // 记录面板初始值
        _initScale = storyPanel.localScale;
        _initPos = storyPanel.position;
        InitializedUI();
    }

    private void OnEnable()
    {
        if (gameObject.activeInHierarchy && index == 0) return;

        // 重置所有变量
        index = 0;
        isPlaying = false;
        // 重置面板位置缩放
        storyPanel.localScale = _initScale;
        storyPanel.position = _initPos;
        // 重置UI显隐
        InitializedUI();
    }

    void InitializedUI()
    {
        dialogueImage.gameObject.SetActive(true);
        clickText.gameObject.SetActive(true);
        zoomTarget.gameObject.SetActive(false);
        playerImage.gameObject.SetActive(false);

        if (dialogueSprites != null && dialogueSprites.Length > 0)
        {
            dialogueImage.sprite = dialogueSprites[index];
        }
    }
    // ======================
    // 你的原始 Update 代码 100% 保留
    // ======================
    void Update()
    {
        if (!isPlaying && Input.GetMouseButtonDown(0))
        {
            index++;

            if (index < dialogueSprites.Length)
            {
                dialogueImage.sprite = dialogueSprites[index];
            }
            else
            {
                isPlaying = true;
                StartCoroutine(PlayStory());
            }
        }
    }

    // ======================
    // 你的原始 协程代码 100% 保留
    // ======================
    IEnumerator PlayStory()
    {
        dialogueImage.gameObject.SetActive(false);
        clickText.gameObject.SetActive(false);
        playerImage.gameObject.SetActive(true);

        Vector3 startScale = storyPanel.localScale;
        Vector3 targetScale = zoomTarget.localScale;
        Vector3 startPos = storyPanel.position;
        Vector3 targetPos = zoomTarget.position;

        float t = 0;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / zoomDuration);
            storyPanel.localScale = Vector3.Lerp(startScale, targetScale, lerp);
            storyPanel.position = Vector3.Lerp(startPos, targetPos, lerp);
            yield return null;
        }

        storyPanel.localScale = targetScale;
        storyPanel.position = targetPos;
        gameObject.SetActive(false);
    }
}