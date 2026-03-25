using UnityEngine;

// 对话框配置类，可挂载在角色身上，或做成ScriptableObject
[System.Serializable]
public class DialogBoxConfig : MonoBehaviour
{
    // 对话框预制体（每个角色可以不同）
    public GameObject dialogBox;
}