using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class MonsterController : MonoBehaviour
{
    public Tilemap tilemap;
    private Map map;
    public int currentHealth;
    [SerializeField]
    private float acceleration = 1.5f;

    [SerializeField]
    public int maxHealth;

    public GameObject player;

    private SpriteRenderer sprite;
    private Animator animator;
    private Rigidbody2D rigidBody;
    
    

    void Start()
    {
        map = tilemap.GetComponent<TilemapScript>().map;
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var start = gameObject.GetPositionInTilemap(tilemap);
        Vector3 nextPos = FindPath(start, map.playerPosition);
        nextPos = gameObject.GetWorldPositionFromTilemap(tilemap, nextPos);

        if (nextPos - transform.position != Vector3.zero)
            animator.SetBool("IsRun", true);
        else
            animator.SetBool("IsRun", false);

        if (nextPos.x - transform.position.x != 0)
            sprite.flipX = (nextPos.x - transform.position.x) < 0;
        
        transform.position = Vector3.MoveTowards(transform.position, nextPos, acceleration * Time.deltaTime);
    }

    public void TakeDamage()
    {
        StartCoroutine(Damag());
    }

    private Vector2 FindPath(Vector2 start, Vector2 end)
    {
        var track = new Dictionary<Vector2, Vector2>();
        track[start] = new Vector2(999999, 999999);
        var queue = new Queue<Vector2>();
        var visited = new HashSet<Vector2>();
        visited.Add(start);
        queue.Enqueue(start);
        while (queue.Count != 0)
        {
            var point = queue.Dequeue();

            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            {
                if (Math.Abs(dx) + Math.Abs(dy) != 1)
                    continue;
                var newPoint = new Vector2(point.x + dx, point.y + dy);
                if (visited.Contains(newPoint) || !map.Contains(newPoint) ||
                    map.map[(int) newPoint.x, (int) newPoint.y] != CellType.Empty)
                {
                    continue;
                }

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
        {
            partItem = end + new Vector2(1, 0);
        }
        else
        {
            partItem = end + new Vector2(-1, 0);
        }

        var result = new List<Vector2>();

        while (partItem != new Vector2(999999, 999999))
        {
            result.Add(partItem);
            partItem = track[partItem];
        }

        result.Reverse();
        if (result.Count >= 2)
            return result[1];
        return start;
    }
    
    private IEnumerator Damag()
    {
        yield return new WaitForSeconds(0.5f);
        currentHealth -= 5;
        Debug.Log("HIT");
        var force = new Vector3(sprite.flipX ? 40f : -40f, 0, 0);
        rigidBody.AddForce(force, ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            animator.SetBool("IsDeath", true);
            Destroy(gameObject);
        }
    }
}