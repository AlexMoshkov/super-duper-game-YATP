using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PathFinder
{
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, CellType[,] map)
    {
        var track = new Dictionary<Vector2Int, Vector2Int>();
        track[start] = new Vector2Int(99999, 99999);
        var queue = new Queue<Vector2Int>();
        var visited = new HashSet<Vector2Int>();
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
                var newPoint = new Vector2Int(point.x + dx, point.y + dy);
                if (visited.Contains(newPoint) || 
                    !Contains(newPoint, map) ||
                    map[newPoint.x, newPoint.y] != CellType.Empty)
                    continue;
                track[newPoint] = point;
                visited.Add(newPoint);
                queue.Enqueue(newPoint);
            }
            if (track.ContainsKey(end))
                break;
        }
        if (!track.ContainsKey(end))
            return new List<Vector2Int>() {};

        var partItem = end;
        var result = new List<Vector2Int>();
        while (partItem != new Vector2Int(99999, 99999))
        {
            result.Add(partItem);
            partItem = track[partItem];
        }
        result.Reverse();
        
        return result;
    }
    
    private bool Contains(Vector2Int point, CellType[,] map)
    {
        return point.x >= 0 && point.x < map.GetLength(0) &&
               point.y >= 0 && point.y < map.GetLength(1);
    }
}
