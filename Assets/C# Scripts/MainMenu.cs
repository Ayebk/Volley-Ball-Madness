using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public UnityEngine.UI.Button FindAMatchButton;
    public UnityEngine.UI.Button StatisticsButton;
    public UnityEngine.UI.Button FriendListButton;
    public UnityEngine.UI.Button SignOutButton;

 
    public GameObject MainMenuCanvas;
    public GameObject FindAMatchCanvas;
    public GameObject FriendListCanvas;
    public GameObject StatisticsCanvas;

    void Start()
    {
        FindAMatchButton.onClick.AddListener(DisplayFindAMatch);
        FriendListButton.onClick.AddListener(DisplayFriendList);
        StatisticsButton.onClick.AddListener(DisplayStatistics);
        SignOutButton.onClick.AddListener(SignOut);
        
    }


    void DisplayFindAMatch() 
    {
        MainMenuCanvas.SetActive(false);
        FindAMatchCanvas.SetActive(true);
    }

    void DisplayFriendList() 
    {
        MainMenuCanvas.SetActive(false);
        FriendListCanvas.SetActive(true);
    }

    void DisplayStatistics() 
    {
        MainMenuCanvas.SetActive(false);
        StatisticsCanvas.SetActive(true);
    }

    void SignOut() 
    {
        SceneManager.LoadScene("Login");
    }
    
}
