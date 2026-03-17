using UnityEngine;

public class PointGizmo : MonoBehaviour
{
    public float size = 0.3f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, size);
    }
}