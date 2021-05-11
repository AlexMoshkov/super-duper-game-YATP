using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    [SerializeField] public float attackDelay = 0.5f;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject player;
    [SerializeField] private Image manaBar;
    private float manaCount;
    private float manaCost;
    private int attackDmg;
    private BoxCollider2D colider;
    private Animator playerAnimator;
    private float timeLeft;
    private bool isAttacking = false;
    private AttackType attackType;
    
    private void Start()
    {
        manaCount = 1f;
        colider = GetComponent<BoxCollider2D>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        timeLeft = attackDelay;
    }

    private void Update()
    {
        attackType = Input.GetKeyDown(KeyCode.A) ? AttackType.NormalAttack : AttackType.NoAttack;
        if (attackType == AttackType.NoAttack)
            attackType = Input.GetKeyDown(KeyCode.W) ? AttackType.HeavyAttack : AttackType.NoAttack;
        if (attackType == AttackType.NoAttack)
            attackType = Input.GetKeyDown(KeyCode.Q) ? AttackType.SpecialAttack : AttackType.NoAttack;

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
                    attackDmg = 5;
                    break;
                case AttackType.HeavyAttack:
                    timeLeft = 1f;
                    triggerAttack = "heavy";
                    attackDmg = 8;
                    break;
                case AttackType.SpecialAttack:
                    manaCost = 0.3f;
                    attackDmg = 0;
                    if (manaCount - manaCost > 0)
                    {
                        timeLeft = 1f;
                        triggerAttack = "special";
                        Quaternion rotate = Quaternion.identity;
                        if (player.GetComponent<SpriteRenderer>().flipX)
                            rotate.z = 10000f;
                        Instantiate(fireBall, shootPosition.position, rotate);
                        manaCount -= manaCost;
                    }
                    break;
            }
            
            playerAnimator.SetTrigger(triggerAttack);
            
            var list = new List<Collider2D>();
            colider.OverlapCollider(new ContactFilter2D(), list);
            
            foreach (var collider in list)
                if (collider.tag == "Enemy")
                    collider.GetComponentInParent<MonsterController>().TakeDamage(attackDmg);
            manaBar.fillAmount = manaCount;
        }
    }
}

