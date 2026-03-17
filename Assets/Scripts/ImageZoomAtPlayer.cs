using UnityEngine;

public class ImageZoomAtPlayer : MonoBehaviour
{
    public Transform player;
    public RectTransform imageUI;
    public Camera cam;

    public float zoomSpeed = 2f;
    public float maxScale = 2f;
    public float minScale = 0.5f;

    private bool zoomIn = true;

    void Update()
    {
        // 1. 玩家世界坐标 → 屏幕坐标
        Vector3 screenPos = cam.WorldToScreenPoint(player.position);

        // 2. UI移动到玩家位置
        imageUI.position = screenPos;

        // 3. 放大缩小
        float scale = imageUI.localScale.x;

        if (zoomIn)
        {
            scale += zoomSpeed * Time.deltaTime;
            if (scale >= maxScale) zoomIn = false;
        }
        else
        {
            scale -= zoomSpeed * Time.deltaTime;
            if (scale <= minScale) zoomIn = true;
        }

        imageUI.localScale = new Vector3(scale, scale, 1);
    }
}