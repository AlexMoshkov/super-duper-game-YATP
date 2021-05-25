using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogsScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text textBox;
    [SerializeField] private Image goblin;
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private string[] dialogs;

    [SerializeField] private GameObject kingSpirit;
    [SerializeField] private int level;
    [SerializeField] private GameObject goblins;
    [SerializeField] private Image bonn;
    [SerializeField] private TrainingController training;
    [SerializeField] private GameObject bossImage;
    [SerializeField] private Image player;

    public int index;
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
        GetNextEvent(index);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNextEvent(++index);
            //Debug.Log(index);
        }
    }

    private void GetNextEvent(int indx)
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
                if (level == 2)
                    goblins.SetActive(true);
                else
                    goblin.enabled = true;
                if (index == 8 && level == 2)
                {
                    foreach (var obj in objects)
                        obj.SetActive(true);
                }
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'K':
                kingSpirit.GetComponent<SpriteRenderer>().enabled = true;
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'M':
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'B':
                bonn.enabled = true;
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'S':
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case 'L':
                textBox.alignment = TextAnchor.UpperLeft;
                textBox.text = text.Substring(1);
                break;
            case 'R':
                textBox.alignment = TextAnchor.UpperRight;
                textBox.text = text.Substring(1);
                break;
            case '_':
                if (level == 1 && index == 12)
                {
                    //Debug.Log("vision");
                    objects[0].GetComponent<MonsterController>().vision = 15;
                    goblin.enabled = false;
                }
                if (level == 2)
                    goblins.SetActive(false);
                if (level == 3)
                    bonn.enabled = false;

                if (level == 3 && index == 5)
                {
                    foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.GetComponent<MonsterController>().vision = 20;
                    }
                }
                
                if (index == 4 && level == 1) //open arrows
                {
                    StartCoroutine(training.ChangeSprite(training.arrows));
                }
                
                if (index == 12 && level == 1) //open A
                {
                    StartCoroutine(training.ChangeSprite(training.buttonA));
                }
                
                if (index == 13 && level == 1) //open S
                {
                    StartCoroutine(training.ChangeSprite(training.buttonS));
                }
                
                if (index == 3 && level == 2) //open W
                {
                    StartCoroutine(training.ChangeSprite(training.buttonD));
                }

                if (index == 11 && level == 3)
                {
                    kingSpirit.GetComponent<King>().enabled = true;
                }

                if (level == 4)
                {
                    bossImage.SetActive(false);
                    player.enabled = false;
                }

                

                index++;
                currentDialog++;
                Time.timeScale = 1;
                
                if (level == 4 && index == 18)
                {
                    SceneManager.LoadScene("mainMenu");
                }
                
                canvas.enabled = false;
                break;
        }
    }

    private IEnumerator KingRun()
    {
        kingSpirit.GetComponent<SpriteRenderer>().flipX = false;
        kingSpirit.transform.position = Vector3.MoveTowards(kingSpirit.transform.position,
            kingSpirit.transform.position + new Vector3(10, 0, 0), 2.5f * Time.deltaTime);
        yield return new WaitForSeconds(10f);
    }
    
}
