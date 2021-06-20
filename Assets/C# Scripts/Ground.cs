using Assets.C__Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject _cloudParticlePrefab;

    public GameObject Camera;
    public Text Textscore;
    public float timer;
    public static bool BackToMain ;
    public int Score = 0;
    public int PlayerZone;
    string currentScoreKey = "currentScore";
    string HighestScoreKey = "HighestScoreKey";

     void Start()
    {
        SoundManger.PlaySound("Ready");
        SoundManger.PlaySound("Game");

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
        if (ball != null && ball.Hit == false && ball._birdWasLaunched)
        {
            if (PlayerZone != ball.ControlingPlayer )
            {
                SoundManger.PlaySound("Ground");
                SoundManger.PlaySound("Score");
                cameraShake(ball);
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
                ball.Hit = true;
                yield return new WaitForSeconds(2.0f);
                ball.ResetBall(true);
            }
            else
            {
                SoundManger.PlaySound("Ground");
                Instantiate(_cloudParticlePrefab, ball.transform.position, Quaternion.LookRotation(Vector3.up));
                ball.Hit = true;
                yield return new WaitForSeconds(2.0f);
                ball.ResetBall(true);
            }

        }

    }

    private void cameraShake(Ball ball) 
    {
        float shakeDuration;
        switch (ball.PowerUpType) 
        {
            case "Heavy": { shakeDuration = 0.5f; } break;
            case "Killer": { shakeDuration = 0.25f; } break;
            default: shakeDuration = 0.25f; break;
        }


        Camera.GetComponent<CameraShake>().ShakeDuration = shakeDuration;
    }

}
