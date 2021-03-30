using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highscore;

    public SpriteRenderer ground;

    public void StartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

     void Start()
    {
        Ground.BackToMain = true;

        
        highscore.text = "Score : " + PlayerPrefs.GetInt("HighestScoreKey").ToString();




    }


    public void QuitGame()
    {
        Application.Quit();

    }
}
