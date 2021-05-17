using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BarrelsScript : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    private Map map;
    void Start()
    {
        map = tilemap.GetComponent<TilemapScript>().map;
        for (var i = 0; i < transform.childCount; ++i)
        {
            var position = transform.GetChild(i).position;
            var positionInTilemap = map.GetPositionInTilemap(tilemap, position);
            map.map[(int) positionInTilemap.x, (int) positionInTilemap.y] = CellType.Barrel;
            Debug.Log(positionInTilemap);
        }
    }
}
//TODO:переместить два метода в tilemap
//TODO:сделать кое-где Vector2Int