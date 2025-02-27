using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 1)] public float smoothTime;
    public Transform playerTransform;

    private void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;
        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);
        pos.z = transform.position.z;

        this.transform.position = pos;
    }
}
