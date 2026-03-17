using UnityEngine;
using UnityEngine.UI;

public class ScrollClick : MonoBehaviour
{
    public Sprite[] dialogueSprites;

    private Image img;
    private int index = 0;

    void Start()
    {
        img = GetComponentInChildren<Image>();
        img.sprite = dialogueSprites[index];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            index++;

            if (index < dialogueSprites.Length)
            {
                img.sprite = dialogueSprites[index];
            }
            else
            {
                gameObject.SetActive(false); // 对话结束
                // 通知 GameManager 触发胜利
                GameManager.Instance.TriggerVictory();
            }
        }
    }
}