using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBottle : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    private void FixedUpdate()
    {
        var colliders = new List<Collider2D>();
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<PlayerController>().HPCount += 7;
                Destroy(gameObject);
            }
        }
    }
}
