using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShoot : MonoBehaviour
{
    private float speed = 5f;

    private void Start()
    {
        Destroy(gameObject, 3f);
        //StartCoroutine(Delete());
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private IEnumerator Delete()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.GetComponentInParent<MonsterController>().currentHealth >= 0)
        {
            collision.GetComponentInParent<MonsterController>().TakeDamage(11);
            Destroy(gameObject);
        }
        if (collision.tag == "Boss" && collision.GetComponentInParent<BossController>().HPFinalStage >= 0)
        {
            collision.GetComponentInParent<BossController>().TakeDamage(11);
            Destroy(gameObject);
        }
    }
}
