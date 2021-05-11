using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameObjectExtension
{
    public static Vector2 GetPositionInTilemap(this GameObject obj, Tilemap tilemap)
    {
        var x = obj.transform.position.x;
        var y = obj.transform.position.y;
        var positionInTilemap = tilemap.GetCellCenterLocal(new Vector3Int((int) Math.Floor(x), (int) Math.Floor(y), 0));
        positionInTilemap -= tilemap.origin;
        positionInTilemap.x = (int)Math.Floor(positionInTilemap.x);
        positionInTilemap.y = (int)Math.Floor(positionInTilemap.y);
        return new Vector2(positionInTilemap.x, positionInTilemap.y);
    }

    public static Vector3 GetWorldPositionFromTilemap(this GameObject obj, Tilemap tilemap, Vector2 pos)
    {
        pos += new Vector2(tilemap.origin.x, tilemap.origin.y);
        pos.x += 0.5f;
        pos.y += 0.5f;
        return new Vector3(pos.x, pos.y, -2);
    }
}