using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableItem : MonoBehaviour
{
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    void Start()
    {
        GameManager.Instance.RegisterMovable(this);
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetItem()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
