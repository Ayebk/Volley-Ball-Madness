using Assets.C__Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAMatch : MonoBehaviour
{
    public UnityEngine.UI.Button FindAMatchButton;
    public UnityEngine.UI.Button InviteAFriendButton;
    public UnityEngine.UI.Button BackButton;
    public GameObject FindAMatchCanvas;
    public GameObject MainMenuCanvas;
    public GameObject TcpClientHandler;
    public GameObject UdpClient;

    // Start is called before the first frame update
    void Start()
    {
        FindAMatchButton.onClick.AddListener(FindMatch);
        BackButton.onClick.AddListener(DisplayMainMenu);
        TcpClientHandler.GetComponent<TcpClientHandler>().onFoundMatch = ConnectToFoundMatch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayMainMenu() 
    {
        FindAMatchCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    void FindMatch()
    {
        TcpClientHandler.GetComponent<TcpClientHandler>().SendFindAMatch();
        Debug.Log("searching for a match");
    }

    void ConnectToFoundMatch(string gameId)
    {
        UdpClient.GetComponent<UdpClientHandler>().ConnectUdp();
        Player.GameRoomId = gameId;
        UdpClient.GetComponent<UdpClientHandler>().SendUdpMessage("CONNECT " + Player.Name + " " + gameId,false);
    }
}
