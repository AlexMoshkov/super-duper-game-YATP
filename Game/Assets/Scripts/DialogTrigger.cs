using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Text textBox;
    private void FixedUpdate()
    {
        var colliders = new List<Collider2D>();
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("MainCamera"))
            {
                canvas.enabled = true;
                textBox.GetComponent<DialogsScript>().objects = objects;
                Destroy(gameObject);
            }
        }
    }
}
