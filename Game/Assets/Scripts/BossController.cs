using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossController : MonoBehaviour
{
    [SerializeField] private float firstStageDelay;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject fireFloor;
    private bool isFinalStage = false;
    private float timeLeft;

    private int currentVariant = 1;

    private Map map;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = firstStageDelay;
        map = tilemap.GetComponent<TilemapScript>().map;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinalStage)
        {
            if (timeLeft > 0)
                timeLeft -= Time.deltaTime;
            else
            {
                
                switch (currentVariant)
                {
                    case 1:
                        Debug.Log("start" + map.playerPosition);
                        currentVariant++;
                        Variant1();
                        break;
                    case 2:
                        currentVariant++;
                        Variant2();
                        break;
                    case 3:
                        currentVariant++;
                        Variant3();
                        break;
                    case 4:
                        currentVariant++;
                        Variant4();
                        break;
                    case 5:
                        currentVariant = 1;
                        Variant5();
                        break;
                }
                timeLeft = firstStageDelay;
            } 
        }
    }

    private void FirstStage()
    {
        
    }

    private void Variant1()
    {
        for (var x = 9; x <= 24; x++)
        {
            for (var y = 10; y <= 13; y++)
            {
                if (x == 9 || x == 24 || y == 10 || y == 13)
                {
                    Instantiate(fireFloor, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)), transform.rotation).SetActive(true);
                }
            }
        }
    }

    private void Variant2()
    {
        
    }

    private void Variant3()
    {
        
    }

    private void Variant4()
    {
        
    }

    private void Variant5()
    {
        
    }
}
