using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
public class Map
{
    public readonly CellType[,] map;
    public Vector2 playerPosition;

    public Map(CellType[,] map, Vector2 playerPos)
    {
        this.map = map;
        playerPosition = playerPos;
    }

    public bool Contains(Vector2 point)
    {
        return point.x >= 0 && point.x < map.GetLength(0) &&
                point.y >= 0 && point.y < map.GetLength(1);
    }
}