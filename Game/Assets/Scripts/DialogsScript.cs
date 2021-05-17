using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class DialogsScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text textBox;
    [SerializeField] private Image goblin;
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private string[] dialogs;

    [SerializeField] private GameObject kingSpirit;
    
    private int index;
    private int currentDialog;
    public GameObject[] objects;
    void Start()
    {
        index = 0;
        textBox.text = dialogs[0];
        currentDialog = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canvas.enabled) return;
        Time.timeScale = 0;
        UpdateText(index);
        if (Input.anyKeyDown)
        {
            UpdateText(++index);
            Debug.Log(index);
        }
    }

    private void UpdateText(int indx)
    {
        var text = dialogs[indx];
        var specialSymbol = text[0];

        switch (specialSymbol)
        {
            case 'P':
                textBox.alignment = TextAnchor.UpperLeft;
                textBox.text = text.Substring(1);
                break;
            case 'G':
                goblin.enabled = true;
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'K':
                kingSpirit.GetComponent<SpriteRenderer>().enabled = true;
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'Q':
                kingSpirit.GetComponent<SpriteRenderer>().enabled = false;
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case '_':
                if (currentDialog == 1)
                {
                    objects[0].GetComponent<MonsterController>().vision = 15;
                    goblin.enabled = false;
                }
                index++;
                currentDialog++;
                Time.timeScale = 1;
                canvas.enabled = false;
                break;
        }
    }
}
