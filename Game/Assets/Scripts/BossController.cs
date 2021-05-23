using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class BossController : MonoBehaviour
{
    [SerializeField] private float firstStageDelay;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fireFloor;
    [SerializeField] private GameObject[] goblins;
    
    
    private bool isFinalStage = false;
    private float timeLeft;
    private int currentVariant = 1;
    private Map map;

    private int deathCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = firstStageDelay;
        map = tilemap.GetComponent<TilemapScript>().map;
        goblins[0].SetActive(true);
        goblins[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinalStage)
        {
            TakeHitFirstPStage();
            FirstStage();
        }
        else
        {
            
        }
    }

    private void FirstStage()
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

    private void TakeHitFirstPStage()
    {
        foreach (var goblin in goblins)
        {
            if (goblin.activeSelf && goblin.GetComponent<MonsterController>().currentHealth <= 0)
            {
                animator.SetTrigger("TakeHit");
                Debug.Log("Take Hit Boss");
                goblin.GetComponent<MonsterController>().SpawnHealthBottle();
                goblin.SetActive(false);
                deathCount++;
            }
        }

        if (deathCount == 2)
        {
            goblins[2].SetActive(true);
            goblins[3].SetActive(true);
        }
        if (deathCount == 4)
        {
            goblins[4].SetActive(true);
            goblins[5].SetActive(true);
        }

        if (deathCount == 6)
        {
            goblins[6].SetActive(true);
            goblins[7].SetActive(true);
            goblins[8].SetActive(true);
        }

        if (deathCount == 9)
        {
            goblins[9].SetActive(true);
            goblins[10].SetActive(true);
            goblins[11].SetActive(true);
        }

        if (deathCount == goblins.Length)
        {
            animator.SetTrigger("finalFirstStage");
            isFinalStage = true;
        }
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
        for (var x = 9; x <= 24; x++)
        {
            for (var y = 10; y <= 13; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Instantiate(fireFloor, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)), transform.rotation).SetActive(true);
                }
            } 
        }
    }

    private void Variant3()
    {
        for (var x = 9; x <= 24; x++)
        {
            for (var y = 10; y <= 13; y++)
            {
                if ((IsBetween(x, 9, 11) && IsBetween(y, 11, 13)) || 
                    (IsBetween(x, 13, 15) && IsBetween(y, 10, 12)) ||
                    (IsBetween(x, 17, 19) && IsBetween(y, 11, 13)) ||
                    (IsBetween(x, 21, 23) && IsBetween(y, 10, 12)))
                    Instantiate(fireFloor, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)), transform.rotation).SetActive(true);
            } 
        }
    }

    private void Variant4()
    {
        for (var x = 9; x <= 24; x++)
        {
            for (var y = 10; y <= 13; y++)
            {
                if (!((IsBetween(x, 10, 11) && IsBetween(y, 11, 12)) || 
                    (IsBetween(x, 15, 16) && IsBetween(y, 11, 12)) ||
                    (IsBetween(x, 20, 21) && IsBetween(y, 11, 12))))
                    Instantiate(fireFloor, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)), transform.rotation).SetActive(true);
            } 
        }
    }

    private void Variant5()
    {
        for (var x = 9; x <= 24; x++)
        {
            for (var y = 10; y <= 13; y++)
            {
                if ((x + y) % 2 != 0)
                {
                    Instantiate(fireFloor, map.GetWorldPositionFromTilemap(tilemap, new Vector2(x, y)), transform.rotation).SetActive(true);
                }
            } 
        }
    }

    private bool IsBetween(int x, int a, int b)
    {
        return (x >= a && x <= b);
    }
}
