using System;
using System.Collections;
using UnityEngine;

public class ScoreWall : MonoBehaviour
{
    [Header("配置")]
    public int requiredScore = 100; // 在 Inspector 里可以随时改成 200 或 500
    [TextArea] public string failMessage = "You need 100 score to break this wall.";
    [Header("淡出动画")]
    public float fadeTime = 0.5f; // 淡出时长

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 只检测玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 获取当前分数
            int currentScore = ScoreManager.Instance.GetCurrentScore();

            // 2. 判断分数是否足够
            if (currentScore >= requiredScore)
            {
                StartCoroutine(FadeAndHide());
            }
            else
            {
                UIManager.Instance.ShowDialogue(gameObject,failMessage);
            }
        }
    }

    // 3. 离开时关闭对话框
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 玩家撞墙后发现过不去，转身离开时立即隐藏提示
            UIManager.Instance.HideDialogue();
        }
    }

    // 淡出协程
    IEnumerator FadeAndHide()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        float alpha = color.a;

        // 逐渐变透明
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeTime;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // 完全透明后，执行隐藏
        GetComponent<ConsumableItem>().Collect();

        // 重置透明度（下次复活用）
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1);
    }
}