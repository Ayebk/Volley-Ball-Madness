using Assets.C__Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TcpClientHandler : MonoBehaviour
{
    private TcpClient tcpClient;
    private byte[] bytesToReceive = new byte[1024 * 2];
    private byte[] bytesToSend = new byte[1024 * 2];
    private NetworkStream stream;

    public delegate void OnSuccessDelegate();
    public delegate void OnFoundMatch(string roomId);
    public OnSuccessDelegate onLoginSuccess;
    public OnSuccessDelegate onRegisterSuccess;
    public OnFoundMatch onFoundMatch;

    void Start()
    {
    }

    void Update()
    {
            if (tcpClient != null && tcpClient.Connected && tcpClient.Available != 0)
            {
                int i = stream.Read(bytesToReceive, 0, bytesToReceive.Length);
                if (i != 0)
                {
                    string dataToReceive = Encoding.ASCII.GetString(bytesToReceive, 0, i);
                    switch (dataToReceive.Split()[0])
                    {
                        case "OKLOGIN":
                            {
                                onLoginSuccess();
                                DisconnectFromServer();
                            }
                            break;

                        case "OKREGISTER":
                            {
                                onRegisterSuccess();
                                DisconnectFromServer();
                            }
                            break;
                         case "FOUNDMATCH": 
                            {
                               if (Player.PlayerNumber != 1)
                               {
                                Player.PlayerNumber = 2;
                               }
                               onFoundMatch(dataToReceive.Split()[1]);
                               DisconnectFromServer();
                            }
                            break;
                         case "WAIT":
                           {
                                Player.PlayerNumber = 1;
                           }break;
                    }

                }
            }
    }

    public void SendMessageToServer(string messageToSend)
    {
        ConnectToServer();
        if (stream != null && stream.CanWrite)
        {
            bytesToSend = Encoding.ASCII.GetBytes(messageToSend);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
            stream.Flush();
            Debug.Log("Sending: " + messageToSend);
        }
    }

    public void SendLoginInformation(string name, string password)
    {
        string message = "LOGIN " + name + " " + password;
        Player.Name = name;
        SendMessageToServer(message);
    }

    public void SendRegisterRequest(string name, string password) 
    {
        string message = "REGISTER " + name + " " + password;
        SendMessageToServer(message);
    }

    public void SendFindAMatch() 
    {
        string message = "FINDMATCH " + Player.Name;
        SendMessageToServer(message);
    }

    public void DisconnectFromServer()
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
        }
    }

    public void ConnectToServer()
    {
        tcpClient = new TcpClient("10.0.0.9", 2000);
        stream = tcpClient.GetStream();
    }
}
