using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float acceleration = 2f;
    public Rigidbody2D rigidBodyComponent;
    
    private Animator animator;
    private SpriteRenderer sprite;
    
    public bool attack0;
    void Awake()
    {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        animator.SetBool("IsRun", false);
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            Run();
    }
    private void Run()
    {
        var up = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        var left = Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        var down = Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        var right = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
       
        var moveVector = new Vector3(left + right, up + down, 0);
        sprite.flipX = moveVector.x < 0;
        animator.SetBool("IsRun", true);
        transform.position += moveVector * acceleration * Time.deltaTime;
    }
}