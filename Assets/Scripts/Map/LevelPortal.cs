using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    public enum PortalType { Next, Previous, Specific }

    [Header("传送设置")]
    public PortalType type = PortalType.Next;
    public int targetLevelIndex; // 仅当选择 Specific 时有效
    private bool isPortal = false;
    public Transform linkedPortal; // 可选：链接的传送点

    private void Awake()
    {
        isPortal = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (isPortal) return;
        // 只有玩家能触发传送
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case PortalType.Next:
                    MapManager.Instance.NextLevel();
                    break;
                case PortalType.Previous:
                    MapManager.Instance.PreviousLevel();
                    break;
                case PortalType.Specific:
                    MapManager.Instance.InitializeLevel(targetLevelIndex);
                    break;
            }

            // 传送后将玩家位置重置到新地图的起点（可选）
            ResetPlayerPosition(other.transform);
        }
        isPortal = true;
    }

    private void ResetPlayerPosition(Transform playerTransform)
    {
        // 简单做法：回到原点，或者你可以给每个Level设置一个SpawnPoint点位
        playerTransform.position = linkedPortal.position;
    }
}