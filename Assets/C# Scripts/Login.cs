
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public UnityEngine.UI.Button LoginButton;
    public UnityEngine.UI.Button SignUpButton;
    public GameObject LoginCanvas;
    public GameObject RegisterCanvas;
    public GameObject TcpClientHandler;

    // Start is called before the first frame update
    void Start()
    {
        LoginButton.onClick.AddListener(LogIn);
        SignUpButton.onClick.AddListener(DisplayRegisterCanvas);
        TcpClientHandler.GetComponent<TcpClientHandler>().onLoginSuccess = LogInSuccess;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogIn()
    {
        string username = Username.text;
        string password = Password.text;
        TcpClientHandler.GetComponent<TcpClientHandler>().SendLoginInformation(username,password);
    }

    /// <summary>
    /// This function hides the Login canvas and displays the Register canvas
    /// </summary>
    public void DisplayRegisterCanvas() 
    {
        LoginCanvas.SetActive(false);
        RegisterCanvas.SetActive(true);
    }

    public void LogInSuccess()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
