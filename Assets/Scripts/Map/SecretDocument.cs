using UnityEngine;

public class SecretDocument : MonoBehaviour
{
    [Header("配置")]
    public AudioClip winSound; // 胜利音效（可选）

    [Header("卷轴")]
    public GameObject ScrollMask;

    [Header("玩家提示")]
    public GameObject interactionUI; // 拖入刚才创建的 InteractionPrompt

    private bool isPlayerInZone = false;
    private void Start()
    {
        if (interactionUI != null) interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (!isPlayerInZone) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            ScrollMask.SetActive(true); // 开启卷轴

            // 3. 销毁文件本身（视觉上被拿走了）
            //Destroy(gameObject);
            GetComponent<ConsumableItem>().Collect();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. 播放胜利音效
            if (winSound != null)
            {
                // 使用 PlayClipAtPoint 防止物体销毁后声音中断
                AudioSource.PlayClipAtPoint(winSound, transform.position);
            }

            // 2. 显示提示（如果有的话）
            if (interactionUI != null) interactionUI.SetActive(true);

            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 玩家离开时，隐藏提示
            if (interactionUI != null) interactionUI.SetActive(false);
            isPlayerInZone = false;
        }
    }
}