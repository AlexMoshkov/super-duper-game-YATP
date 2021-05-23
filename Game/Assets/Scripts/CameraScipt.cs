using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScipt : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private float acceleration = 2f;
    private float downBorder = -8f;
    private float upBorder = 0.8f;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + 2 < player.transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, transform.position.y, -10), acceleration * Time.deltaTime);
        }

        if (transform.position.x - 2 > player.transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, transform.position.y, -10), acceleration * Time.deltaTime);
        }
        
        if (transform.position.y - 2.5 > player.transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, downBorder, -10), acceleration * Time.deltaTime);
        }
        
        if (transform.position.y - 1.5 < player.transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(player.transform.position.x, upBorder, -10), acceleration * Time.deltaTime);
        }
    }
}
