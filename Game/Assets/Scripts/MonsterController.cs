using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class MonsterController : MonoBehaviour
{
    public Tilemap tilemap;
    private Map map;
    public int currentHealth;
    [SerializeField] private float acceleration = 1.5f;
    [SerializeField] public int vision;
    [SerializeField] private bool isDropHealthBottle;
    [SerializeField] public int maxHealth;
    [SerializeField] private GameObject attackZone;
    [SerializeField] private bool isDestroyed;

    [SerializeField] private int attackDamage = 3;

    private PathFinder pathFinder = new PathFinder();
    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D rigidBody;
    
    private bool isAttacking;
    private float timeLeft;
    private bool takingDamage = false;

    private void Attack()
    {
        if (isAttacking)
            timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
            isAttacking = false;

        if (!isAttacking)
        {
            isAttacking = true;
            timeLeft = 1.5f;
            animator.SetTrigger("attack");
            
            var list = new List<Collider2D>();
            attackZone.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), list);
            foreach (var collider in list)
            {
                if (collider.CompareTag("Player"))
                    collider.GetComponentInParent<PlayerController>().TakeDamage(attackDamage);
            }
        }
    }

    void Start()
    {
        map = tilemap.GetComponent<TilemapScript>().map;
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentHealth <= 0 || takingDamage) return;
        
        var nextPosInTilemap = GetNextPosition(map.playerPosition);
        
        Debug.Log(nextPosInTilemap);
        if (nextPosInTilemap == new Vector2Int(99999, 99999))
        {
            animator.SetBool("IsRun", false);
            return;
        }
        var curPos = map.GetPositionInTilemap(tilemap, transform.position);
        animator.SetBool("IsRun", curPos != nextPosInTilemap);
        
        var nextPos = map.GetWorldPositionFromTilemap(tilemap, nextPosInTilemap);
        nextPos.z = -1.5f;
        sprite.flipX = (map.playerPosition.x <= curPos.x);
        if (curPos + Vector2Int.left == map.playerPosition || curPos + Vector2Int.right == map.playerPosition)
            Attack();

        transform.position = Vector3.MoveTowards(transform.position, nextPos, acceleration * Time.deltaTime);
    }

    private Vector2Int GetNextPosition(Vector2Int playerPos)
    {
        var currentPos = map.GetPositionInTilemap(tilemap, transform.position);
        var leftTarget = playerPos + Vector2Int.left;
        var rightTarget = playerPos + Vector2Int.right;
        var leftPath = pathFinder.FindPath(currentPos, leftTarget, map.map);
        var rightPath = pathFinder.FindPath(currentPos, rightTarget, map.map);
        if ((leftPath.Count <= rightPath.Count && leftPath.Count != 0) || rightPath.Count == 0)
        {
            return GetNextPositionInPath(currentPos, leftPath);
        }
        return GetNextPositionInPath(currentPos, rightPath);
    }

    private Vector2Int GetNextPositionInPath(Vector2Int currentPos, List<Vector2Int> path)
    {
        if (path.Count >= vision)
            return new Vector2Int(99999, 99999);
        return path.Count >= 2 ? path[1] : currentPos;
    }

    public void TakeDamage(int damage)
    {
        if (damage != 0 && currentHealth > 0)
            StartCoroutine(Damage(damage));
    }

    private IEnumerator Damage(int dmg)
    {
        yield return new WaitForSeconds(0.5f);
        takingDamage = true;
        currentHealth -= dmg;
        animator.SetTrigger("takeHit");
        if (currentHealth > 0 && dmg > 0)
        {
            var force = new Vector3(sprite.flipX ? 20f : -20f, 0, 0);
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            animator.SetBool("IsRun", false);
            animator.SetBool("IsDeath", true);

            foreach (var collider in gameObject.GetComponents<BoxCollider2D>())
            {
                collider.enabled = false;
            }
            yield return new WaitForSeconds(1.5f);
            SpawnHealthBottle();
            if (isDestroyed)
            {
                Destroy(gameObject, 4f);
            }
                
        }
        takingDamage = false;
    }

    public void SpawnHealthBottle()
    {
        if (isDropHealthBottle)
            Instantiate(GameObject.Find("Health Bottle"), 
                new Vector3(transform.position.x, transform.position.y, -2),
                transform.rotation);
    }
}