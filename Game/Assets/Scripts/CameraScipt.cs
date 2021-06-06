using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScipt : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    [SerializeField] private BoxCollider2D leftBorder;
    [SerializeField] private BoxCollider2D rightBorder;
    [SerializeField] private BoxCollider2D cameraCollider;
    [SerializeField] private GameObject[] enemies;

    public bool isTriggeredNow = false;
        
    private float acceleration = 1.7f;
    private BoxCollider2D playerCollider;

    private bool isLevelStart = false;
    
    void Start()
    {
        playerCollider = player.GetComponent<BoxCollider2D>();
        
        IgnoreEnemyCollider();
        
        Physics2D.IgnoreCollision(leftBorder, playerCollider, true);
        Physics2D.IgnoreCollision(rightBorder, playerCollider, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggeredNow)
        {
            StopTriggerCamera();
            return;
        }
        
        moveCamera();
    }

    private void StopTriggerCamera()
    {
        var colliders = new List<Collider2D>();
        cameraCollider.OverlapCollider(new ContactFilter2D(), colliders);
        var enemyCount = colliders.Count(collider => collider.CompareTag("Enemy") && collider.GetComponent<MonsterController>().currentHealth > 0);
        Debug.Log(enemyCount);
        if (enemyCount == 0)
            isTriggeredNow = false;
    }
    
    private void moveCamera()
    {
        if (transform.position.x < player.transform.position.x + 3)
        {
            isLevelStart = true;
            Physics2D.IgnoreCollision(leftBorder, playerCollider, false);
            Physics2D.IgnoreCollision(rightBorder, playerCollider, false);
        }

        if (transform.position.x < player.transform.position.x || !isLevelStart)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, transform.position.y, -10), acceleration * Time.deltaTime);
        }
    }
    

    private void IgnoreEnemyCollider()
    {
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.name);
            var enemyColliders = enemy.GetComponents<BoxCollider2D>();
            foreach (var collider in enemyColliders)
            {
                Physics2D.IgnoreCollision(leftBorder, collider, true);
                Physics2D.IgnoreCollision(rightBorder, collider, true);
            }
        }
    }
}

