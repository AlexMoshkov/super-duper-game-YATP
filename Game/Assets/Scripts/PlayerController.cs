using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float acceleration = 2f;
    public Rigidbody2D rigidBodyComponent;

    public AttackType attack;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject attackZone;

    private BoxCollider2D attackRect;
    [SerializeField]
    private bool isAttacking = false;

    private float attackTimer = 0f;
    private float attackReset = 0.5f;

    void Awake()
    {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        
        attackZone = GameObject.Find("Attack Zone");
        attackRect = attackZone.GetComponent<BoxCollider2D>();
        attackRect.enabled = false;
        attack = AttackType.Empty;
        //attackZone.SetActive(false);
    }
    
    void Update()
    {
        var attack0 = Input.GetKey(KeyCode.A);
        
        animator.SetBool("IsRun", false);
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            Run();
        if (attack0)
        {
            animator.SetTrigger("attack");;
            attackRect.enabled = true;
        }
        else
        {
            attackRect.enabled = false;
        }
        
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

    IEnumerator c_Attack()
    {
        isAttacking = true;
        attackRect.enabled = true;
        yield return null;              // !!!!!
        Debug.Log(attackRect.enabled);
        attackRect.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Debug.Log(attackRect.enabled);
        isAttacking = false;
    }
}

public enum AttackType
{
    Empty,
    NormalAttack
}
