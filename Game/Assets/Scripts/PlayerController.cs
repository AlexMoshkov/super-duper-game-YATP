using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Image HPBar;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float speedManaRegeneration = 0.5f;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Image manaBar;
    
    private float HPCount;
    private float manaCost = 0.3f;
    
    public Rigidbody2D rigidBodyComponent;
    
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject attackZone;

    private bool isAttacking = false;
    private float timeLeft;
    
    private Map map;
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private GameObject HealthBottle;
    
    void Awake()
    {
        HPCount = 30;
        manaBar.fillAmount = 1f;
        rigidBodyComponent = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attackZone = GameObject.Find("Attack Zone");
        timeLeft = attackDelay;
        
        map = tilemap.GetComponent<TilemapScript>().map;
    }
    
    void Update()
    {
        if (!animator.GetBool("IsDeath"))
        {
            animator.SetBool("IsRun", false);
            MakeAttack();
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
                Run();
        }
        HPBar.fillAmount = HPCount / 30;
        manaBar.fillAmount += speedManaRegeneration * Time.deltaTime;
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

    private void MakeAttack()
    {
        var attackType = Input.GetKeyDown(KeyCode.A) ? AttackType.NormalAttack : AttackType.NoAttack;
        if (attackType == AttackType.NoAttack)
            attackType = Input.GetKeyDown(KeyCode.W) ? AttackType.HeavyAttack : AttackType.NoAttack;
        if (attackType == AttackType.NoAttack)
            attackType = Input.GetKeyDown(KeyCode.Q) ? AttackType.SpecialAttack : AttackType.NoAttack;

        if (isAttacking)
            timeLeft -= Time.deltaTime;
        
        if (timeLeft < 0) 
            isAttacking = false;
        
        if (attackType != AttackType.NoAttack && !isAttacking)
        {
            isAttacking = true;

            var triggerAttack = "";
            var attackDmg = 0;
            switch (attackType)
            {
                case AttackType.NormalAttack:
                    timeLeft = 1f;
                    triggerAttack = "normal";
                    attackDmg = 5;
                    break;
                case AttackType.HeavyAttack:
                    manaCost = 0.1f;
                    if (manaBar.fillAmount - manaCost < 0)
                        break;
                    timeLeft = 2f;
                    triggerAttack = "heavy";
                    attackDmg = 8;
                    manaBar.fillAmount -= manaCost;
                    break;
                case AttackType.SpecialAttack:
                    manaCost = 0.3f;
                    if (manaBar.fillAmount - manaCost < 0)
                        break;
                    timeLeft = 5f;
                    triggerAttack = "special";
                    Quaternion rotate = Quaternion.identity;
                    if (sprite.flipX)
                        rotate.z = 10000f;
                    Instantiate(fireBall, shootPosition.position, rotate);
                    manaBar.fillAmount -= manaCost;
                    break;
            }

            if (triggerAttack == "")
                return;
            animator.SetTrigger(triggerAttack);
            
            var list = new List<Collider2D>();
            var playerCollider = attackZone.GetComponent<BoxCollider2D>();
            playerCollider.OverlapCollider(new ContactFilter2D(), list);
            foreach (var collider in list)
            {
                if (collider.tag == "Enemy")
                    collider.GetComponentInParent<MonsterController>().TakeDamage(attackDmg);
                else if (collider.tag != "Player") 
                {
                    Debug.Log("AAAA");
                    var x = (int) map.playerPosition.x + (sprite.flipX ? -1 : 1);
                    var y = (int) map.playerPosition.y;
                    if (map.map[x, y] == CellType.Barrel)
                    {
                        Debug.Log(x + " " + y);
                        map.map[x, y] = CellType.Empty;

                        // for (var xx = tilemap.origin.x; xx < tilemap.origin.x + tilemap.size.x; xx++)
                        // for (var yy = tilemap.origin.y; yy < tilemap.origin.y + tilemap.size.y; yy++)
                        // {
                        //     if (tilemap.GetTile(new Vector3Int(xx ,yy, 0)) != null)
                        //         Debug.Log(tilemap.GetTile(new Vector3Int(xx ,yy, 0)).name + " " + xx + " " + yy);
                        // }
                        

                        tilemap.SetTile(tilemap.origin + new Vector3Int(x, y, 0), null);
                        Instantiate(HealthBottle, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)),
                            Quaternion.identity);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(Damage(damage));
    }
    
    private IEnumerator Damage(int damage)
    {
        if (animator.GetBool("IsDeath") || isAttacking)
            yield break;
        yield return new WaitForSeconds(0.5f);
        HPCount -= damage;
        animator.SetTrigger("takeHit");
        
        // if (HPCount > 0 && damage > 0)
        // {
        //     var force = new Vector3(sprite.flipX ? 20f : -20f, 0, 0);
        //     rigidBody.AddForce(force, ForceMode2D.Impulse);
        // }
            
        if (HPCount <= 0)
        {
            animator.SetBool("IsRun", false);
            animator.SetBool("IsDeath", true);
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
            Debug.Log("YOU ARE DEAD");
        }
    }
}

public enum AttackType
{
    NoAttack,
    NormalAttack,
    HeavyAttack,
    SpecialAttack
}