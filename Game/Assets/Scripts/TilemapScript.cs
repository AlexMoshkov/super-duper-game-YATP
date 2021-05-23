﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    [SerializeField] private GameObject[] monsters;
    public Tilemap tilemap;
    public Map map;
    private GameObject player;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        player = GameObject.Find("Player");
        var cellmap = GetMap();
        map = new Map(cellmap, tilemap, player.transform.position);
    }

    private void Update()
    {
        map.playerPosition = map.GetPositionInTilemap(tilemap, player.transform.position);
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
            if (tile != null)
                result[x, y] = CellType.Occupied;
            else result[x, y] = CellType.Empty;
        }
        return result;
    }
}