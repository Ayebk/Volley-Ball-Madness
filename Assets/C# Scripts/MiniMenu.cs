using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniMenu : MonoBehaviour
{
    
    public GameObject minimenu;
    public bool ispasued = false;
    // Start is called before the first frame update
    int flagstart = 0;
    public int countdownTime;
    public Text countdownDisplay;


    public void Pasuegame()
    {
        minimenu.SetActive(true);
        Time.timeScale = 0f;
        ispasued = true;
    }
    public void Resumegame()
    {
        minimenu.SetActive(false);
        Time.timeScale = 1f;
        ispasued = false;
    }

    void Start()
    {
        Resumegame();
        StartCoroutine(CountdownToStart());
        

    }



    IEnumerator CountdownToStart()
    {

        if (flagstart == 0)
        {
            while (countdownTime > 0)
            {
                countdownDisplay.text = countdownTime.ToString();
                yield return new WaitForSeconds(1f);
                countdownTime--;

            }
            if (countdownTime == 0)
            {
                flagstart = 1;
                countdownDisplay.text = "Go!";
                Resumegame();
            }

        }
       
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispasued)
            {
                Resumegame();
            }
            else
            {
                Pasuegame();
            }
        }
    }
  
}
