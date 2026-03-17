using UnityEngine;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    public Sprite[] dialogueSprites;

    private Image img;
    private int index = 0;

    void Start()
    {
        img = GetComponent<Image>();
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
            }
        }
    }
}