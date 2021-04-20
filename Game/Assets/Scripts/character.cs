using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character
{
    public Vector3 position;

    public Character(Vector3 position)
    {
        this.position = position;
    }

    public Vector2 GetPositionInTilemap(Tilemap tilemap)
    {
        var x = position.x;
        var y = position.y;
        var positionInTilemap = tilemap.GetCellCenterLocal(new Vector3Int((int) Math.Floor(x), (int) Math.Floor(y), 0));
        positionInTilemap -= new Vector3(-10,-2,0);
        positionInTilemap.x = (int)Math.Floor(positionInTilemap.x);
        positionInTilemap.y = (int)Math.Floor(positionInTilemap.y);
        return new Vector2(positionInTilemap.x, positionInTilemap.y);
    }

    public void UpdatePosition(Vector3 pos)
    {
        position = pos;
    }
    
}
