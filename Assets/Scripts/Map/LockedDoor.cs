using System.Collections;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // 尝试消耗钥匙
                if (player.TryConsumeKey())
                {
                    StartCoroutine(DelayDestroy());
                }
                else
                {
                    // 失败：显示英文提示
                    UIManager.Instance.ShowDialogue("Locked. Need a key.");
                }
            }
        }
    }

    // 核心逻辑：玩家停止碰撞（离开门）时，关闭对话框
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 只有当门还没被销毁（即没有钥匙没打开）时，
            // 玩家撞了一下离开，需要立即隐藏提示 "Locked..."
            UIManager.Instance.HideDialogue();
        }
    }

    IEnumerator DelayDestroy()
    {
        // 成功：显示英文提示
        UIManager.Instance.ShowDialogue("Used a key to open the door.");

        // 播放开门音效或动画 (可选)
        // AudioSource.PlayClipAtPoint(openSound, transform.position);
        // 延时1秒
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideDialogue();
        //Destroy(gameObject);
        GetComponent<ConsumableItem>().Collect();
    }
}