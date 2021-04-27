using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    public Tilemap tilemap;
    public Map map;
    private GameObject player;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        player = GameObject.Find("Player");
        var cellmap = GetMap();
        map = new Map(cellmap, GetPlayerPosition());
    }

    private void Update()
    {
        map.playerPosition = player.GetPositionInTilemap(tilemap);
    }

    private CellType[,] GetMap()
    {
        var bounds = tilemap.cellBounds;
        var result = new CellType[bounds.size.x, bounds.size.y]; 
        var allTiles = tilemap.GetTilesBlock(bounds);
        for (var x = 0; x < bounds.size.x; x++)
        for (var y = 0; y < bounds.size.y; y++)
        {
            var tile = allTiles[x + y * bounds.size.x];
            if (tile != null && tile.name == "Barrel")
                result[x, y] = CellType.Barrel;
            else result[x, y] = CellType.Empty;
        }
        return result;
    }

    private Vector2 GetPlayerPosition()
    {
        var x = player.transform.position.x;
        var y = player.transform.position.y;
        var position = tilemap.GetCellCenterLocal(new Vector3Int((int) Math.Floor(x), (int) Math.Floor(y), 0));
        position.x -= -10;
        position.y -= -2;
        position.x = (int)Math.Floor(position.x);
        position.y = (int)Math.Floor(position.y);
        return new Vector2(position.x, position.y);
    }
}