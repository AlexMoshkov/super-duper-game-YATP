using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedArea : MonoBehaviour
{
    [SerializeField] private bool forceDown = false;
    [SerializeField] private bool isEscape = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEscape && GameObject.Find("Bonn").GetComponent<MonsterController>().currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {
            var force = new Vector3(other.GetComponent<SpriteRenderer>().flipX ? 30f : -30f, forceDown ? -20f : 0, 0);
            other.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            other.GetComponent<Animator>().SetTrigger("takeHit");
        }
    }
}
