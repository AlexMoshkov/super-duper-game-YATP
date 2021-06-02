using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private BoxCollider2D collider;
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
            if (coll.name == "Right Border")
            {
                coll.gameObject.GetComponentInParent<CameraScipt>().isTriggeredNow = true;
                Debug.Log("YE");
                Destroy(gameObject);
            }
        }
        
    }
}
