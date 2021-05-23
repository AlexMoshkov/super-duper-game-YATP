using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOnDeath : MonoBehaviour
{
    public GameObject onDeathMenuUI;
    public Image Hp;
    void Start()
    {
        onDeathMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp.fillAmount == 0f)
        {
            StartCoroutine(GetMenu());
        }
    }

    private IEnumerator GetMenu()
    {
        yield return new WaitForSeconds(2.5f);
        onDeathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }
}
