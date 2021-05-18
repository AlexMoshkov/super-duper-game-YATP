using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private string nextLevel;
    private void FixedUpdate()
    {
        var list = new List<Collider2D>();
        collider.OverlapCollider(new ContactFilter2D(), list);
        foreach (var collider in list)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("go to exit");
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
