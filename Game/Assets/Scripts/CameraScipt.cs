using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraScipt : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    [SerializeField] private BoxCollider2D leftBorder;
    [SerializeField] private BoxCollider2D rightBorder;
    
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
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Physics2D.IgnoreCollision(leftBorder, enemy.GetComponent<BoxCollider2D>(), true);
            Physics2D.IgnoreCollision(rightBorder, enemy.GetComponent<BoxCollider2D>(), true);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "CameraTrigger")
            Debug.Log("YEAAAA");
    }
}

