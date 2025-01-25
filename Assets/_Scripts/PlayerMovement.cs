using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sp = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _invent = GetComponent<Inventory>();
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
        if (Input.GetMouseButton(0))
        {
            _anim.SetTrigger("attack");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryShowing = !inventoryShowing;
        }
        _invent.inventoryUI.SetActive(inventoryShowing);

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedIndex < _invent.inventoryWidth-1)
                selectedIndex++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedIndex > 0)
                selectedIndex--;
        }

        hotbarSelector.transform.position = _invent.hotbarUISlot[selectedIndex].transform.position;
    }
}
