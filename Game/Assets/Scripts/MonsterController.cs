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

    private int attackDamage = 5;
    
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
                if (collider.tag == "Player")
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
        if (currentHealth > 0 && !takingDamage)
        {
            var start = map.GetPositionInTilemap(tilemap, transform.position);
            Vector3 nextPos = FindPath(start, map.playerPosition);
            if (nextPos != new Vector3(99999, 99999, 0))
            {
                nextPos = map.GetWorldPositionFromTilemap(tilemap, nextPos);
                nextPos.z = -1.5f;
                if (nextPos - transform.position != Vector3.zero)
                    animator.SetBool("IsRun", true);
                else
                    animator.SetBool("IsRun", false);

                if (nextPos.x - transform.position.x != 0)
                    sprite.flipX = (nextPos.x - transform.position.x) < 0;

                if (!animator.GetBool("IsRun"))
                    Attack();

                transform.position = Vector3.MoveTowards(transform.position, nextPos, acceleration * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage != 0 && currentHealth > 0)
            StartCoroutine(Damage(damage));
    }

    private Vector2 FindPath(Vector2 start, Vector2 end)
    {
        var track = new Dictionary<Vector2, Vector2>();
        track[start] = new Vector2(99999, 99999);
        var queue = new Queue<Vector2>();
        var visited = new HashSet<Vector2>();
        visited.Add(start);
        queue.Enqueue(start);
        while (queue.Count != 0)
        {
            var point = queue.Dequeue();

            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    continue;
                var newPoint = new Vector2(point.x + dx, point.y + dy);
                if (visited.Contains(newPoint) || !map.Contains(newPoint) ||
                    map.map[(int) newPoint.x, (int) newPoint.y] != CellType.Empty)
                    continue;

                track[newPoint] = point;
                visited.Add(newPoint);
                queue.Enqueue(newPoint);
            }

            if (track.ContainsKey(end + new Vector2(1, 0)) || track.ContainsKey(end + new Vector2(-1, 0))) break;
        }

        if (!track.ContainsKey(end + new Vector2(1, 0)) && !track.ContainsKey(end + new Vector2(-1, 0)))
            return start;

        var partItem = end;

        if (track.ContainsKey(end + new Vector2(1, 0)))
            partItem = end + new Vector2(1, 0);
        else
            partItem = end + new Vector2(-1, 0);

        var result = new List<Vector2>();

        while (partItem != new Vector2(99999, 99999))
        {
            result.Add(partItem);
            partItem = track[partItem];
        }

        if (result.Count >= vision)
            return new Vector2(99999, 99999);

        result.Reverse();
        if (result.Count >= 2)
            return result[1];
        return start;
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
        }
        takingDamage = false;
    }

    private void SpawnHealthBottle()
    {
        if (isDropHealthBottle)
            Instantiate(GameObject.Find("Health Bottle"), 
                new Vector3(transform.position.x, transform.position.y, -2),
                transform.rotation);
    }
}