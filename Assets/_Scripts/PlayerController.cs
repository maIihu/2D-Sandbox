using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool onGround;
    public float jumpForce;
    public float horizontal;
    
    private Rigidbody2D _rb;
    private SpriteRenderer _sp;
    private Animator _anim;
    private Inventory _invent;
    
    private bool inventoryShowing = false;
    
    public GameObject hotbarSelector;
    private int selectedIndex = 0;
    
    public ItemClass itemSelected = null;

    public bool hit;

    public GameObject healthBar;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _invent = GetComponent<Inventory>();
    }

    private void Start()
    {
        Instantiate(healthBar, this.transform);
        SelectedItem(selectedIndex);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");
        hit = Input.GetMouseButton(0);
        
        
        Vector2 movement = new Vector2(horizontal * moveSpeed, _rb.velocity.y);
        
        if (horizontal > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal < 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }

        _rb.velocity = movement;
    }

    private void Update()
    {
        _anim.SetFloat("horizontal", horizontal);
        _anim.SetBool("hit", hit);

        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryShowing = !inventoryShowing;
        }
        _invent.inventoryUI.SetActive(inventoryShowing);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex < _invent.inventoryWidth - 1)
            {
                selectedIndex++;
                SelectedItem(selectedIndex);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
                SelectedItem(selectedIndex);
            }
        }
        
        
    }

    private void SelectedItem(int index)
    {
        ItemClass item = null;
        hotbarSelector.transform.position = _invent.hotbarUISlot[index].transform.position;
        Transform transformItem = this.transform.Find("Arm").transform.GetChild(0);
         try
         {
             item = _invent.inventorySlots[index, _invent.inventoryHeight - 1].item;
             
             if(item.tool) // weapon
             {
                 transformItem.GetComponent<SpriteRenderer>().sprite =
                     item.itemSprite;
                 PolygonCollider2D collider = transformItem.GetComponent<PolygonCollider2D>();
                 if (collider != null)
                 {
                     Destroy(collider);
                 }
                 collider = transformItem.gameObject.AddComponent<PolygonCollider2D>();
                 if (collider != null)
                 {
                     collider.isTrigger = true;
                 }
             }
             else // block
             {
                 transformItem.GetComponent<SpriteRenderer>().sprite = null;
                 if (transformItem.GetComponent<PolygonCollider2D>() != null)
                 {
                     Destroy(transformItem.GetComponent<PolygonCollider2D>());
                 }
             }


         }
         catch
         {
             item = null;
             if (transformItem.GetComponent<PolygonCollider2D>() != null)
             {
                 Destroy(transformItem.GetComponent<PolygonCollider2D>());
             }
         }

         itemSelected = item;
    }
    
}
