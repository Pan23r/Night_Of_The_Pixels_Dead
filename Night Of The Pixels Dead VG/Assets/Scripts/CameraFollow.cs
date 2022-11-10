using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float minPositionX;
    public float maxPositionX;
    public float minPositionY;
    public float maxPositionY;

    private void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, minPositionX, maxPositionX), Mathf.Clamp(transform.position.y, minPositionY, maxPositionY)); 
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
