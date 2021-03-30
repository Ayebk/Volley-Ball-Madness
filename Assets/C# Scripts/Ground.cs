using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject _cloudParticlePrefab;

    public Text Textscore;
    public float timer;
    bool Hit = false;
    public static bool BackToMain ;
    public int Score = 0;
    string currentScoreKey = "currentScore";
    string HighestScoreKey = "HighestScoreKey";


    void Start()
    {
       // GetComponent<Timer>().timerIsRunning = true;
        if (BackToMain)
        {
            PlayerPrefs.SetInt(currentScoreKey, 0);
        }
        //Get the highScore from player prefs if it is there, 0 otherwise.
        Score = PlayerPrefs.GetInt(currentScoreKey, 0);
    }




    public IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.collider.GetComponent<Ball>();
        if (ball != null && Hit == false)
        {
            BackToMain = false;
            Instantiate(_cloudParticlePrefab, ball.transform.position, Quaternion.LookRotation(Vector3.up));
            Score = Score + 1;
            PlayerPrefs.SetInt(currentScoreKey, Score);
            if (PlayerPrefs.GetInt(HighestScoreKey) < PlayerPrefs.GetInt(currentScoreKey))
            {
                PlayerPrefs.SetInt(HighestScoreKey, Score);
            }
            PlayerPrefs.Save();
            Textscore.text = "Score: " + Score.ToString();
            Hit = true;
            yield return new WaitForSeconds(2.0f);




            string currentSceneName = SceneManager.GetActiveScene().name;

       //     timer = GetComponent<Timer>().timeRemaining;
        //    GetComponent<Timer>().timerIsRunning = true ;
            SceneManager.LoadScene(currentSceneName);
 
        }

    }

}
