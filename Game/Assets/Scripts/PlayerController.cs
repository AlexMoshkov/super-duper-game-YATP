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
    private AttackType attackType;
    
    private Map map;
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private GameObject HealthBottle;
    
    void Awake()
    {
        attackType = AttackType.NoAttack;
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
        //if (Time.timeScale == 0) return;
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
        if (!isAttacking)
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
    }

    private void MakeAttack()
    {
        attackType = Input.GetKeyDown(KeyCode.A) ? AttackType.NormalAttack : AttackType.NoAttack;
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
                    manaCost = 0.25f;
                    if (manaBar.fillAmount - manaCost < 0)
                        break;
                    timeLeft = 0.8f;
                    triggerAttack = "heavy";
                    attackDmg = 8;
                    manaBar.fillAmount -= manaCost;
                    break;
                case AttackType.SpecialAttack:
                    manaCost = 0.6f;
                    if (manaBar.fillAmount - manaCost < 0)
                        break;
                    timeLeft = 0.8f;
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
            
            var colliders = new List<Collider2D>();
            var playerCollider = attackZone.GetComponent<BoxCollider2D>();
            playerCollider.OverlapCollider(new ContactFilter2D(), colliders);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                    collider.GetComponentInParent<MonsterController>().TakeDamage(attackDmg);
                else if (collider.CompareTag("Barrel")) 
                {
                    Destroy(collider.gameObject, 0.6f);
                    var positionInTilemap = map.GetPositionInTilemap(tilemap, collider.transform.position);
                    map.map[(int) positionInTilemap.x, (int) positionInTilemap.y] = CellType.Empty;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        //TODO:переписать эту тупость :)
        StartCoroutine(Damage(damage));
    }
    
    private IEnumerator Damage(int damage)
    {
        if (animator.GetBool("IsDeath"))
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