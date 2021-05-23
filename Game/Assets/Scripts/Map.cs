﻿using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map
{
    public readonly CellType[,] map;
    public Vector2Int playerPosition;

    public Map(CellType[,] map, Tilemap tilemap, Vector3 playerPos)
    {
        this.map = map;
        playerPosition = GetPositionInTilemap(tilemap, playerPos);
    }

    public bool Contains(Vector2 point)
    {
        return point.x >= 0 && point.x < map.GetLength(0) &&
                point.y >= 0 && point.y < map.GetLength(1);
    }

    public Vector2Int GetPositionInTilemap(Tilemap tilemap, Vector3 position)
    {
        var x = position.x;
        var y = position.y;
        var posInTilemap = tilemap.GetCellCenterLocal(new Vector3Int((int) Math.Floor(x), (int) Math.Floor(y), 0));
        posInTilemap -= tilemap.origin;
        posInTilemap.x = (int)Math.Floor(posInTilemap.x);
        posInTilemap.y = (int)Math.Floor(posInTilemap.y);
        return new Vector2Int((int)posInTilemap.x, (int)posInTilemap.y);
    }

    public Vector3 GetWorldPositionFromTilemap(Tilemap tilemap, Vector2 position)
    {
        position += new Vector2(tilemap.origin.x, tilemap.origin.y);
        position += new Vector2(0.5f, 0.5f);
        return new Vector3(position.x, position.y, -2);
    }
    
}