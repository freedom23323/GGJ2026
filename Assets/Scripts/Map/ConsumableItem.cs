using UnityEngine;

public class ConsumableItem : MonoBehaviour
{
    // 记录初始状态
    private void OnEnable()
    {
        // 游戏开始或切换关卡时，将自己注册到重置列表中
        GameManager.Instance.RegisterConsumable(this);
    }

    public void Collect()
    {
        // 玩家触碰时，不再销毁，而是隐藏
        gameObject.SetActive(false);
    }

    public void ResetItem()
    {
        // 重置时重新显示
        gameObject.SetActive(true);
    }
}