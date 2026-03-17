using UnityEngine;

public class SecretDocument : MonoBehaviour
{
    [Header("配置")]
    public AudioClip winSound; // 胜利音效（可选）

    [Header("卷轴")]
    public GameObject ScrollMask;

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

            ScrollMask.SetActive(true); // 开启卷轴

            // 3. 销毁文件本身（视觉上被拿走了）
            //Destroy(gameObject);
            GetComponent<ConsumableItem>().Collect();
        }
    }
}