using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterController : MonoBehaviour
{
    public Tilemap tilemap;
    private Map map;
    private float acceleration = 1.5f;

    private SpriteRenderer sprite;

    void Start()
    {
        map = tilemap.GetComponent<TilemapScript>().map;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector3 nextPos = FindPath(gameObject.GetPositionInTilemap(tilemap), map.playerPosition);
        nextPos = gameObject.GetWordPositionFromTilemap(tilemap, nextPos);
        nextPos -= transform.position;
        transform.position += new Vector3(nextPos.x, nextPos.y, 0).normalized * acceleration * Time.deltaTime;
        sprite.flipX = nextPos.x < 0;
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
                    map.map[(int)newPoint.x, (int)newPoint.y] == CellType.Barrel)
                {
                    continue;
                }
                track[newPoint] = point;
                visited.Add(newPoint);
                queue.Enqueue(newPoint);
            }

            if (track.ContainsKey(end)) break;
        }

        if (!track.ContainsKey(end))
            return start;
        
        var partItem = end;
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
