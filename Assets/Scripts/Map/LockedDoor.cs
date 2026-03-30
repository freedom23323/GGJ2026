using System.Collections;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("音效设置")]
    public AudioClip Sound; //音效
    [Header("玩家提示")]
    public GameObject interactionUI; // 拖入刚才创建的 InteractionPrompt
    private bool isPlayerInZone = false;
    PlayerController player;

    public string successDialogue= "Used a key to open the door.";
    public string failDialogue = "Locked. Need a key.";
    void Start()
    {
        interactionUI.SetActive(false); // 初始时隐藏提示
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
            // 尝试消耗钥匙
            if (player.TryConsumeKey())
            {
                StartCoroutine(DelayDestroy());
            }
            else
            {
                // 失败：显示英文提示
                UIManager.Instance.ShowDialogue(gameObject, failDialogue);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            interactionUI.SetActive(true); // 显示“按 F”提示
            isPlayerInZone = true;
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
            interactionUI.SetActive(false); // 隐藏“按 F”提示
            isPlayerInZone = false;
        }
    }

    IEnumerator DelayDestroy()
    {
        // 成功：显示英文提示
        UIManager.Instance.ShowDialogue(gameObject, successDialogue);

        // 播放开门音效
        AudioSource.PlayClipAtPoint(Sound, transform.position);
        // 延时1秒
        interactionUI.SetActive(false); // 隐藏“按 F”提示
        yield return new WaitForSeconds(1f);
        UIManager.Instance.HideDialogue();
        //Destroy(gameObject);
        GetComponent<ConsumableItem>().Collect();
    }
}