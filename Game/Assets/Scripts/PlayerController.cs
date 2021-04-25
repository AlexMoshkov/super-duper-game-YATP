using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float acceleration = 2f;
    public Rigidbody2D rigidBodyComponent;
    private Animator animator;
    private SpriteRenderer sprite;
    

    void Start()
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
        var w = Input.GetKey(KeyCode.W) ? 1 : 0;
        var a = Input.GetKey(KeyCode.A) ? -1 : 0;
        var s = Input.GetKey(KeyCode.S) ? -1 : 0;
        var d = Input.GetKey(KeyCode.D) ? 1 : 0;
        var moveVector = new Vector3(a + d, w + s, 0);
        sprite.flipX = moveVector.x < 0;
        animator.SetBool("IsRun", true);
        transform.position += moveVector * acceleration * Time.deltaTime;
    }
}
