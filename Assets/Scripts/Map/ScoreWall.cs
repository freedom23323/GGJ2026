using UnityEngine;

public class ScoreWall : MonoBehaviour
{
    [Header("配置")]
    public int requiredScore = 100; // 在 Inspector 里可以随时改成 200 或 500
    [TextArea] public string failMessage = "You need 100 score to break this wall.";

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
                // --- 成功逻辑 ---
                // 播放破碎音效 (可选)
                // AudioSource.PlayClipAtPoint(breakSound, transform.position);

                // 显示成功提示（可选，或者直接销毁更干脆）
                //UIManager.Instance.ShowDialogue("Power overflowing!");

                // 销毁墙壁
                //Destroy(gameObject);
                GetComponent<ConsumableItem>().Collect();
            }
            else
            {
                UIManager.Instance.ShowDialogue(failMessage);
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
}