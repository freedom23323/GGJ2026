using System.Collections;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("设置")]
    public AudioClip pickupSound; // 可选：拾取音效

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只检测玩家
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // 联动 UI：提示获得钥匙
                UIManager.Instance.ShowDialogue("get a key!");
                // 1. 增加钥匙
                player.AddKey();

                // 2. 播放音效 (如果 AudioManager 有的话，或者直接用 AudioSource)
                if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                StartCoroutine(DelayDestroy());
            
            }
        }
    }
    IEnumerator DelayDestroy()
    {
        // 延时1秒
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideDialogue();
        //Destroy(gameObject);
        GetComponent<ConsumableItem>().Collect();
    }
}