using UnityEngine;

public class SecretDocument : MonoBehaviour
{
    [Header("配置")]
    public AudioClip winSound; // 胜利音效（可选）

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

            // 2. 通知 GameManager 触发胜利
            GameManager.Instance.TriggerVictory();

            // 3. 销毁文件本身（视觉上被拿走了）
            //Destroy(gameObject);
            GetComponent<ConsumableItem>().Collect();
        }
    }
}