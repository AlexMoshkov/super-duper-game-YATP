using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 target;
    [SerializeField] private SpriteRenderer sprite; 
    void Start()
    {
        target = transform.position + new Vector3(12, 0, 0);
        sprite.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 2.3f * Time.deltaTime);
        if (transform.position == target)
        {
            sprite.flipX = true;
            enabled = false;
        }
    }
}
