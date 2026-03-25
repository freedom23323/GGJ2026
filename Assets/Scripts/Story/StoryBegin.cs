using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryBegin : MonoBehaviour
{
    [Header("对话配置")]
    public Sprite[] dialogueSprites;
    public Image dialogueImage;       // 拖入 DialogueImage
    public TextMeshProUGUI clickText; // 拖入 ClickText

    [Header("演出配置")]
    public RectTransform storyPanel;  // 拖入 StoryPanel (要被放大的那一层)
    public RectTransform zoomTarget;  // 拖入 ZoomTarget (提供目标位置和缩放的空节点)

    [Header("演示")]
    public Image playerImage;

    [Header("动画参数")]
    public float zoomDuration = 1.5f;

    private int index = 0;
    private bool isPlaying = false;

    void Start()
    {
        // 初始状态设置
        dialogueImage.gameObject.SetActive(true);
        clickText.gameObject.SetActive(true);
        zoomTarget.gameObject.SetActive(false);
        playerImage.gameObject.SetActive(false);

        if (dialogueSprites != null && dialogueSprites.Length > 0)
        {
            dialogueImage.sprite = dialogueSprites[index];
        }
    }

    void Update()
    {
        // 只有在没播放动画时才接受点击
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

    IEnumerator PlayStory()
    {
        // 1. 关闭对话UI，防止它们跟着 StoryPanel 一起被放大
        dialogueImage.gameObject.SetActive(false);
        clickText.gameObject.SetActive(false);
        playerImage.gameObject.SetActive(true);

        // 2. 缓存初始状态与目标状态
        Vector3 startScale = storyPanel.localScale;
        Vector3 targetScale = zoomTarget.localScale;

        Vector3 startPos = storyPanel.position;
        Vector3 targetPos = zoomTarget.position;

        float t = 0;

        // 3. 执行放大与移动动画
        while (t < zoomDuration)
        {
            t += Time.deltaTime;

            // 使用 SmoothStep 让动画有一个平滑的起步和刹车，手感更好
            float lerp = Mathf.SmoothStep(0, 1, t / zoomDuration);

            storyPanel.localScale = Vector3.Lerp(startScale, targetScale, lerp);
            storyPanel.position = Vector3.Lerp(startPos, targetPos, lerp);

            yield return null;
        }

        // 4. 收尾：强制对齐最终数值，消除浮点数误差
        storyPanel.localScale = targetScale;
        storyPanel.position = targetPos;

        // 5. 动画结束后的处理 (根据你的需求保留或修改)
        gameObject.SetActive(false); // 关闭整个 StoryBeginPanel 
    }
}