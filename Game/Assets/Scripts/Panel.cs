using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject Levels;
    public void PlayPressed()
    {
        SceneManager.LoadScene("Level1");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
    public void FirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }
    public void SecondLevel()
    {
        SceneManager.LoadScene("Level2");
    }
    public void ThirdLevel()
    {
        SceneManager.LoadScene("Level3");
    }    
    public void FourthLevel()
    {
        SceneManager.LoadScene("Level4");
    }
    public void LevelPressed()
    {
        Buttons.SetActive(false);
        Levels.SetActive(true);
    }

    public void BackPressed()
    {
        Buttons.SetActive(true);
        Levels.SetActive(false);
    }
}
