using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackController : MonoBehaviour
{
    private BoxCollider2D colider;
    private Animator playerAnimator;
    private int time;
    private void Start()
    {
        colider = GetComponent<BoxCollider2D>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        var attack0 = Input.GetKeyDown(KeyCode.A);
        if (attack0)
        {
            playerAnimator.SetTrigger("attack");

            var list = new List<Collider2D>();
            colider.OverlapCollider(new ContactFilter2D(), list);
            foreach (var collider in list)
            {
                if (collider.tag == "Enemy")
                {
                    collider.GetComponentInParent<MonsterController>().TakeDamage();
                    Debug.Log("HIT");
                }
            }
        }
    }
}