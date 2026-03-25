using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("提示配置")]
    [TextArea] public string Dialogue = "切换为恶魔试试呢？";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.GetCurrentState() == "DemonState") return;

            UIManager.Instance.ShowDialogue((int)DialogueBoxType.DemonDialogueBox, Dialogue);
        }
    }

    // 离开碰撞 → 延时0.5秒关闭
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 启动协程延迟关闭
            StartCoroutine(HideDialogueDelay(0.5f));
        }
    }

    // 延时隐藏对话框协程
    IEnumerator HideDialogueDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        UIManager.Instance.HideDialogue();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}