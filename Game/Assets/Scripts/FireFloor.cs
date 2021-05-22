using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloor : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Collider2D fireCollider;

    [SerializeField] private float attackDelay;
    [SerializeField] private int damageCount;

    private float timeLeft;

    private bool giveDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;
        else
            Hit();
    }

    private void Hit()
    {
        Debug.Log("READY TO HIT");
        animator.SetTrigger("attack");
        var colliders = new List<Collider2D>();
        fireCollider.OverlapCollider(new ContactFilter2D(), colliders);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player") && !giveDamage)
            {
                collider.GetComponent<PlayerController>().TakeDamage(damageCount);
                giveDamage = true;
            }
        }
        Destroy(gameObject, 2f);
    }
}
