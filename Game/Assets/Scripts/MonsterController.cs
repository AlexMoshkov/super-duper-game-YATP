using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
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

    void Start()
    {
        map = tilemap.GetComponent<TilemapScript>().map;
        sprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3 nextPos = FindPath(gameObject.GetPositionInTilemap(tilemap), map.playerPosition);
        nextPos = gameObject.GetWorldPositionFromTilemap(tilemap, nextPos);
        nextPos -= transform.position;
        //Debug.Log(nextPos);
        transform.position += new Vector3(nextPos.x, nextPos.y, 0).normalized * acceleration * Time.deltaTime;
        sprite.flipX = nextPos.x < 0;
    }

    public void TakeDamage()
    {
        currentHealth -= 5; 
        if (currentHealth < 0)
            Destroy(gameObject);    
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
                    map.map[(int) newPoint.x, (int) newPoint.y] == CellType.Barrel)
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
}
