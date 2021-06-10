using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private GameObject[] enemies = new GameObject[]{};
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var colliders = new List<Collider2D>();
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        foreach (var coll in colliders)
        {
            if (coll.name == "Barrier Right")
            {
                coll.gameObject.GetComponentInParent<CameraScipt>().isTriggeredNow = true;
                ActiveEnemies();
                Debug.Log("YE");
                Destroy(gameObject);
            }
        }
    }

    private void ActiveEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
    
    
}
