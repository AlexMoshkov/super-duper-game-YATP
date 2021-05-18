using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedArea : MonoBehaviour
{
    [SerializeField] private bool forceDown = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var force = new Vector3(other.GetComponent<SpriteRenderer>().flipX ? 30f : -30f, forceDown ? -20f : 0, 0);
            other.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            other.GetComponent<Animator>().SetTrigger("takeHit");
        }
    }
}
