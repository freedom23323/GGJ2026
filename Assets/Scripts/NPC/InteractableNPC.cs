using UnityEngine;
using TMPro;

public class InteractableNPC : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject interactionUI; // 拖入刚才创建的 InteractionPrompt

    [Header("对话配置")]
    [TextArea] public string angelDialogue;
    [TextArea] public string demonDialogue;

    private bool isPlayerInZone = false;

    void Update()
    {
        // 只有当玩家在范围内，才检测按键
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            // 如果对话框当前没开启，则触发对话
            //if (UIManager.Instance.isDialogueActive) return;
            TriggerDialogue();
            // 触发对话后，隐藏“按 F”提示，避免视觉干扰
            interactionUI.SetActive(false);
        }
    }

    private void TriggerDialogue()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        string currentDialogue = (player.GetCurrentState() =="AngelState") ? angelDialogue : demonDialogue;

        UIManager.Instance.ShowDialogue(currentDialogue);
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