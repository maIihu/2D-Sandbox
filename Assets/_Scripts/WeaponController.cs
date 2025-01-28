using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private bool hit;

    private void Update()
    {
        hit = Input.GetMouseButton(0); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hit && other.CompareTag("Enemy"))
        {
            Debug.Log("Hello");
        }
    }
}