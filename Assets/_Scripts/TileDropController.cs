using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropController : MonoBehaviour
{
    public float destroyTime;
    public ItemClass item;
    
    private void Start()
    {
        StartCoroutine(DestroyAfterTime(10f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(DestroyAfterTime(10f));
            if(other.GetComponent<Inventory>().AddItem(item));
                Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
    
}
