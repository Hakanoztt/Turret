using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{

    public Transform playerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Start() {
        offset = transform.position - playerTransform.position;
    }
    void LateUpdate() {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        //transform.LookAt(playerTransform);
    }
}
