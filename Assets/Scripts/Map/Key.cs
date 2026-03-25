using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("设置")]
    public AudioClip pickupSound; // 可选：拾取音效
    [Header("玩家提示")]
    public GameObject interactionUI; // 拖入刚才创建的 InteractionPrompt
    private bool isPlayerInZone = false;
    PlayerController player;
    private void Start()
    {
        if (interactionUI != null) interactionUI.SetActive(false); // 初始时隐藏提示
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.HideDialogue();
        }
        if (!isPlayerInZone) return;
        if (player != null && Input.GetKeyDown(KeyCode.F))
        {
            // 联动 UI：提示获得钥匙
            UIManager.Instance.ShowDialogue(gameObject, "get a key!");
            // 1. 增加钥匙
            player.AddKey();

            // 2. 播放音效 (如果 AudioManager 有的话，或者直接用 AudioSource)
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            StartCoroutine(DelayDestroy());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只检测玩家
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            interactionUI.SetActive(true); // 显示“按 F”提示
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionUI.SetActive(false); // 隐藏“按 F”提示
        isPlayerInZone = false;
    }
    IEnumerator DelayDestroy()
    {
        interactionUI.SetActive(false);
        // 延时1秒
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideDialogue();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ConsumableItem>().Collect();
    }

    //void KeyDestroy()
    //{
    //    interactionUI.SetActive(false);
    //    UIManager.Instance.HideDialogue();
    //    GetComponent<ConsumableItem>().Collect();
    //}
}