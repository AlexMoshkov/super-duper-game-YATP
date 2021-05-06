using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    public float attackDelay = 0.5f;
    
    private BoxCollider2D colider;
    private Animator playerAnimator;
    private float timeLeft;
    private bool isAttacking = false;
    private AttackType attackType;
    
    private void Start()
    {
        colider = GetComponent<BoxCollider2D>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        timeLeft = attackDelay;
    }

    private void Update()
    {
        
        attackType = Input.GetKeyDown(KeyCode.A) ? AttackType.NormalAttack : AttackType.NoAttack;
        if (attackType == AttackType.NoAttack)
            attackType = Input.GetKeyDown(KeyCode.W) ? AttackType.HeavyAttack : AttackType.NoAttack;

        if (isAttacking)
            timeLeft -= Time.deltaTime;

        if (timeLeft < 0) 
            isAttacking = false;
        
        if (attackType != AttackType.NoAttack && !isAttacking)
        {
            isAttacking = true;

            var triggerAttack = "";
            switch (attackType)
            {
                case AttackType.NormalAttack:
                    timeLeft = 0.5f;
                    triggerAttack = "normal";
                    break;
                case AttackType.HeavyAttack:
                    timeLeft = 1f;
                    triggerAttack = "heavy";
                    break;
            }
            
            playerAnimator.SetTrigger(triggerAttack);
            
            var list = new List<Collider2D>();
            colider.OverlapCollider(new ContactFilter2D(), list);
            
            foreach (var collider in list)
                if (collider.tag == "Enemy")
                    collider.GetComponentInParent<MonsterController>().TakeDamage();
        }
    }
}

public enum AttackType
{
    NoAttack,
    NormalAttack,
    HeavyAttack
}