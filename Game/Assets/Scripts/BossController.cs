using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Random = System.Random;


public class BossController : MonoBehaviour
{
    [SerializeField] private float firstStageDelay;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fireFloor;
    [SerializeField] private GameObject[] goblins;
    [SerializeField] private GameObject skeleton;

    [SerializeField] private int HPFinalStage;

    private bool isFinalStage = true;
    private float timeLeft;
    private int currentVariant = 1;
    private Map map;
    private int HPFirstStage;
    private Vector3 target;
    private Random random; 

    private float timeMoving;

    private float timeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = firstStageDelay;
        map = tilemap.GetComponent<TilemapScript>().map;
        //goblins[0].SetActive(true);
        //goblins[1].SetActive(true);
        HPFirstStage = goblins.Length; //TODO: сделать хп бар для первой фазы босса 
        HPFinalStage = 100;
        timeMoving = 3f;
        timeSpawn = 10f;
        random = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (HPFinalStage < 0)
        {
            Debug.Log("YOU WIN");
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
        else
        {
            if (!isFinalStage)
            {
                TakeHit();
                FirstStage();
            }
            else
            {
                RandomMove();
                FirstStage();
                SpawnSkeletons();
            }
        }
    }

    private void SpawnSkeletons()
    {
        if (timeSpawn > 0)
            timeSpawn -= Time.deltaTime;
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length < 3)
        {
            var pos = transform.position;
            pos.z = 2;
            Instantiate(skeleton, pos, transform.rotation).SetActive(true);
            timeSpawn = 10f;
        }
        else
        {
            timeLeft = 10f;
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

    private void RandomMove()
    {
        if (timeMoving > 0)
        {
            timeMoving -= Time.deltaTime;
        }
        else
        {
            var pos = map.GetPositionInTilemap(tilemap, transform.position);
            var nextPos = GetNextPos(pos);
            //Debug.Log(nextPos);
            target = map.GetWorldPositionFromTilemap(tilemap, nextPos);
            timeMoving = 1f;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, 1.2f * Time.deltaTime);
    }

    private Vector2Int GetNextPos(Vector2Int pos)
    {
        while (true)
        {
            var direction = GetRandomDirection();
            //Debug.Log("direction: " + direction);
            var pathLength = GetRandomPathLenght();
            //Debug.Log("Length: " + pathLength);
            var nextPos = pos + direction * pathLength;
            //Debug.Log("nextPos: " + nextPos);
            if (ContainsInZone(nextPos))
            {
                //Debug.Log("GOOD");
                return nextPos;
            }
        }
    }

    private bool ContainsInZone(Vector2Int pos)
    {
        return pos.x >= 9 && pos.x <= 24 && pos.y >= 10 && pos.y <= 13;
    }

    private Vector2Int GetRandomDirection()
    {
        var rand = new Random();
        var number = random.Next(4);
        var result = number switch
        {
            0 => Vector2Int.down,
            1 => Vector2Int.left,
            2 => Vector2Int.up,
            3 => Vector2Int.right,
            _ => Vector2Int.zero
        };
        Debug.Log(number + " " + result);
        return result;
    }

    private int GetRandomPathLenght()
    {
        var rand = new Random();
        return random.Next(1, 6);
    }

    private void TakeHit()
    {
        foreach (var goblin in goblins)
        {
            if (goblin.activeSelf && goblin.GetComponent<MonsterController>().currentHealth <= 0)
            {
                animator.SetTrigger("TakeHit");
                goblin.GetComponent<MonsterController>().SpawnHealthBottle();
                goblin.SetActive(false);
                HPFirstStage--;
            }
        }

        if (HPFirstStage == goblins.Length - 2)
        {
            goblins[2].SetActive(true);
            goblins[3].SetActive(true);
        }
        if (HPFirstStage == goblins.Length - 4)
        {
            goblins[4].SetActive(true);
            goblins[5].SetActive(true);
        }

        if (HPFirstStage == goblins.Length - 6)
        {
            goblins[6].SetActive(true);
            goblins[7].SetActive(true);
            goblins[8].SetActive(true);
        }

        if (HPFirstStage == goblins.Length - 9)
        {
            goblins[9].SetActive(true);
            goblins[10].SetActive(true);
            goblins[11].SetActive(true);
        }

        if (HPFirstStage == 0)
        {
            animator.SetTrigger("finalFirstStage");
            isFinalStage = true;
            transform.position += Vector3.down;
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (damage != 0 && HPFinalStage > 0)
            StartCoroutine(Damage(damage));
    }
    
    private IEnumerator Damage(int dmg)
    {
        yield return new WaitForSeconds(0.5f);
        HPFinalStage -= dmg;
        animator.SetTrigger("TakeHit");
        Debug.Log("HIT");
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
