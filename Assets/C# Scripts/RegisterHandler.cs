using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterHandler : MonoBehaviour
{
    public TMP_InputField Username;
    public TMP_InputField Password;
    public UnityEngine.UI.Button RegisterButton;
    public UnityEngine.UI.Button BackButton;
    public GameObject LoginCanvas;
    public GameObject RegisterCanvas;
    public GameObject TcpClientHandler;

    // Start is called before the first frame update
    void Start()
    {
        RegisterButton.onClick.AddListener(Register);
        BackButton.onClick.AddListener(DisplayLoginCanvas);
        TcpClientHandler.GetComponent<TcpClientHandler>().onRegisterSuccess = RegisterSuccess;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Register() 
    {
        string username = Username.text;
        string password = Password.text;
        TcpClientHandler.GetComponent<TcpClientHandler>().SendRegisterRequest(username, password);
    }

    void RegisterSuccess() 
    {
        Debug.Log("Registered successfully");
    }

    /// <summary>
    /// This function hides the Register canvas and displays the Login canvas
    /// </summary>
    void DisplayLoginCanvas()
    {
        RegisterCanvas.SetActive(false);
        LoginCanvas.SetActive(true);
    }
}
