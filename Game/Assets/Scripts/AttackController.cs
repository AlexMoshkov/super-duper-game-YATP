using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    public float acceleration = 1e-5f;
    
    // Start is called before the first frame update
    private GameObject player;
    private BoxCollider2D zone;
    private AttackType attack; 

    private void Start()
    {
        player = GameObject.Find("Player");
        zone = GameObject.Find("Attack Zone").GetComponent<BoxCollider2D>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Debug.Log("Hellow");
        attack = player.GetComponent<PlayerController>().attack;
        if (attack == AttackType.NormalAttack)
        {
            DoAttack(attack);
            attack = AttackType.Empty;
            gameObject.SetActive(false);
        }
    }

    public void DoAttack(AttackType attack)
    {

        gameObject.SetActive(true);
    }
    
}
