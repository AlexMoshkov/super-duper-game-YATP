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
        
        Vector3 nextPos = GetNextPosition(map.playerPosition);
        Debug.Log(nextPos);
        if (nextPos == new Vector3(99999, 99999, 0)) return;
        
        nextPos = map.GetWorldPositionFromTilemap(tilemap, nextPos);
        nextPos.z = -1.5f;
        
        
        animator.SetBool("IsRun", nextPos - transform.position != Vector3.zero);

        if (nextPos.x - transform.position.x != 0)
            sprite.flipX = (nextPos.x - transform.position.x) < 0;

        if (!animator.GetBool("IsRun"))
            Attack();

        transform.position = Vector3.MoveTowards(transform.position, nextPos, acceleration * Time.deltaTime);
    }

    private Vector2 GetNextPosition(Vector2Int playerPos)
    {
        var currentPos = map.GetPositionInTilemap(tilemap, transform.position);
        var leftTarget = playerPos + Vector2Int.left;
        var rightTarget = playerPos + Vector2Int.right;
        Debug.Log("targets: " + leftTarget + " " + rightTarget);
        var leftPath = pathFinder.FindPath(currentPos, leftTarget, map.map);
        var rightPath = pathFinder.FindPath(currentPos, rightTarget, map.map);
        Debug.Log(rightPath.Count + " " + leftPath.Count);
        if ((leftPath.Count <= rightPath.Count && leftPath.Count != 0) || rightPath.Count == 0)
        {
            Debug.Log("left");
            return GetNextPositionInPath(currentPos, leftPath);
        }
        return GetNextPositionInPath(currentPos, rightPath);
    }

    private Vector2 GetNextPositionInPath(Vector2Int currentPos, List<Vector2Int> path)
    {
        if (path.Count >= vision)
            return new Vector2(99999, 99999);
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
        Debug.Log("HIT");
        Debug.Log(dmg);
        if (currentHealth > 0 && dmg > 0)
        {
            var force = new Vector3(sprite.flipX ? 20f : -20f, 0, 0);
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            animator.SetBool("IsRun", false);
            animator.SetBool("IsDeath", true);

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
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