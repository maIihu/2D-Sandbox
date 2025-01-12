using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    private Vector3 direction;
    public LayerMask layer;
    private RaycastHit2D hit;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            hit = Physics2D.Raycast(mouseWorldPos, Vector2.up, Mathf.Infinity, layer.value);
            if(hit)
                Debug.Log("hello");
        }

    }
}
