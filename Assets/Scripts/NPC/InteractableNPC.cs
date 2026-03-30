using UnityEngine;
using TMPro;

public class InteractableNPC : MonoBehaviour
{
    [Header("玩家提示")]
    public GameObject interactionUI; // 拖入刚才创建的 InteractionPrompt

    [Header("对话配置")]
    [TextArea] public string angelDialogue;
    [TextArea] public string demonDialogue;

    private bool isPlayerInZone = false;

    private void Start()
    {
        if (interactionUI != null) interactionUI.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInZone)
        {
            interactionUI.SetActive(true);
        }
        else
        {
            interactionUI.SetActive(false);
        }
        // 只有当玩家在范围内，才检测按键
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            // 如果对话框当前没开启，则触发对话
            //if (UIManager.Instance.isDialogueActive) return;
            Debug.Log("玩家按下 F 键，触发对话");
            TriggerDialogue();
        }
        if (isPlayerInZone && Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.HideDialogue();
        }
    }

    private void TriggerDialogue()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        string currentDialogueSentence = (player.GetCurrentState() =="AngelState") ? angelDialogue : demonDialogue;

        UIManager.Instance.ShowDialogue(gameObject, currentDialogueSentence);
    }

    // --- 核心逻辑：触发 UI 提示 ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            // 只要玩家靠近，就显示“按 F”
            if (interactionUI != null) interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            // 玩家离开，隐藏提示
            if (interactionUI != null) interactionUI.SetActive(false);
            // 同时也建议关闭对话框
            UIManager.Instance.HideDialogue();
        }
    }
}