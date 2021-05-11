using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image HPBar;
    private float HPCount;
    private float acceleration = 2f;
    public Rigidbody2D rigidBodyComponent;
    
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject attackZone;
    
    public bool attack0;
    void Awake()
    {
        HPCount = 1f;
        rigidBodyComponent = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attackZone = GameObject.Find("Attack Zone");
    }
    
    void Update()
    {
        animator.SetBool("IsRun", false);
        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            Run();
        HPBar.fillAmount = HPCount;
    }
    private void Run()
    {
        var up = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        var left = Input.GetKey(KeyCode.LeftArrow) ? -1 : 0;
        var down = Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        var right = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
       
        var moveVector = new Vector3(left + right, up + down, 0);
        sprite.flipX = moveVector.x < 0;
        if (moveVector.x != 0)
            attackZone.transform.localScale = new Vector3(moveVector.x, 1, 1);
        
        animator.SetBool("IsRun", true);
        transform.position += moveVector * acceleration * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            GetDamage(0.25);
        }
    }
    public void GetDamage(double dmg)
    {
        var newDmg = float.Parse(dmg.ToString());
        StartCoroutine(GetDamage(newDmg));
    }
    public IEnumerator GetDamage(float dmg)
    {
        HPCount -= dmg;
        yield return new WaitForSeconds(1f);
    }
}