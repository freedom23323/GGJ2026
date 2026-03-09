using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int scoreValue = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 调用单例增加分数
            ScoreManager.Instance.AddScore(scoreValue);

            // 可以在这里播放音效或粒子
            //Destroy(gameObject);
            GetComponent<ConsumableItem>().Collect();
        }
    }
}
