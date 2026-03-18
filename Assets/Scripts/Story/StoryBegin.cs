using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoryBegin : MonoBehaviour
{
    [Header("对话图片")]
    public Sprite[] dialogueSprites;
    public Image dialogueImage;

    [Header("演出图片")]
    public Image mapImage;       // 第一张（放大）
    public Image playingImage;   // 第二张（展示）

    private int index = 0;

    [Header("动画参数")]
    public float zoomDuration = 1.5f;
    public float stayDuration = 3f;
    public Transform player;
    public RectTransform rect;
    public RectTransform targetPoint;

    private bool isPlaying = false;

    void SetZoomCenterToPlayer()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(player.position);

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.rect,
            screenPos,
            null,
            out localPoint
        );

        Vector2 pivot = new Vector2(
            (localPoint.x / this.rect.rect.width) + 0.5f,
            (localPoint.y / this.rect.rect.height) + 0.5f
        );

        RectTransform rect = transform as RectTransform;
        rect.pivot = pivot;
    }

    void Start()
    {
        // 初始只显示对话
        dialogueImage.gameObject.SetActive(true);
        mapImage.gameObject.SetActive(true);
        playingImage.gameObject.SetActive(false);

        dialogueImage.sprite = dialogueSprites[index];
        rect= GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            index++;

            if (index < dialogueSprites.Length)
            {
                dialogueImage.sprite = dialogueSprites[index];
            }
            else if (!isPlaying)
            {
                //SetZoomCenterToPlayer();
                StartCoroutine(PlayStory());
                isPlaying = true;
            }
        }
    }

    IEnumerator PlayStory()
    {
        // 关闭对话UI
        dialogueImage.gameObject.SetActive(false);

        // ===== ① 第一张图：放大 =====
        playingImage.gameObject.SetActive(true);


        Vector3 startScale = rect.localScale;
        Vector3 targetScale = targetPoint.localScale;

        Vector3 startPos = rect.position;
        Vector3 targetPos = targetPoint.position;

        float t = 0;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float lerp = t / zoomDuration;

            rect.localScale = Vector3.Lerp(startScale, targetScale, lerp);
            rect.position = Vector3.Lerp(startPos, targetPos, lerp);

            yield return null;
        }

        //// ===== ② 切第二张图 =====

        //mapImage.gameObject.SetActive(false);
        //playingImage.gameObject.SetActive(true);

        //yield return new WaitForSeconds(stayDuration);

        // ===== ③ 结束 =====
        playingImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}