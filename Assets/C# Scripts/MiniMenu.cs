using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMenu : MonoBehaviour
{
    
    public GameObject minimenu;
    public bool ispasued = false;
    // Start is called before the first frame update


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
