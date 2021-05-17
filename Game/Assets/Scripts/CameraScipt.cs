﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // var stop = false;
        // var list = new List<Collider2D>();
        // GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), list);
        // foreach (var collider in list)
        // {
        //     if (collider.tag == "Enemy")
        //         stop = true;
        // }
        //transform.position = new Vector3(player.transform.position.x, 1, -10);
        if (transform.position.x + 2 < player.transform.position.x)
        {
            transform.position += Vector3.right * 2f * Time.deltaTime;
        }

        if (transform.position.x - 2 > player.transform.position.x)
        {
            transform.position += Vector3.left * 2f * Time.deltaTime;
        }
        
    }
}
