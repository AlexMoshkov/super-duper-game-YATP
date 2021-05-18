using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;

    [SerializeField] public Sprite arrows;
    [SerializeField] public Sprite buttonA;
    [SerializeField] public Sprite buttonS;
    [SerializeField] public Sprite buttonD;

    public IEnumerator ChangeSprite(Sprite newSprite)
    {
        renderer.sprite = newSprite;
        yield return new WaitForSeconds(5f);
        renderer.sprite = null;
    }
}
