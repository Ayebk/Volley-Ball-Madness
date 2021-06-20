using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UdpClientHandler : MonoBehaviour
{
    public static GameObject Instance;
    public static UdpClient UdpClient;
    public static IPEndPoint RemoteEndPoint;
    public static Byte[] receiveBytes;
    public static string HostIp = "10.0.0.9";
    public static Int32 HostPort = 65000;
    public static bool ConnectedFlag = false;
    public static bool Connect = false;
    public static bool InGameMode = false;

    public delegate void OnTimeReceived(string time);
    public delegate void OnBallPosition(string position);
    public OnTimeReceived onTimeReceived;
    public OnBallPosition onBallPosition;
    public OnBallPosition onBallReleased;
    public OnBallPosition onBallCollision;
    public OnBallPosition onBallReset;
    public OnBallPosition onBallCurrentPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (UdpClient == null && Connect)
        {
            UdpClient = new UdpClient(65300);
        }
        else if(Connect)
        {
            if (ConnectedFlag && UdpClient.Available != 0)
            {
                receiveBytes = UdpClient.Receive(ref RemoteEndPoint);
                string message = Encoding.ASCII.GetString(receiveBytes);
                CommandHandler(message);
            }
        }
    }

    public void SendUdpMessage(string message,bool threeTimes) 
    {
        byte[] bytesToSend = Encoding.ASCII.GetBytes(message);
        if (ConnectedFlag)
        {
            if (threeTimes)
            {
                for (int i = 0; i < 3; i++)
                {
                    UdpClient.Send(bytesToSend, bytesToSend.Length);
                }
            }
            else
            {
                UdpClient.Send(bytesToSend, bytesToSend.Length);
            }
            
        }
    }

    public void ConnectUdp()
    {
        
        if (!ConnectedFlag)
        {
            if (UdpClient == null)
            {
                try
                {
                    UdpClient = new UdpClient(65300);
                }
                catch (Exception e)
                {
                    UdpClient = new UdpClient(65301);
                }
            }
            UdpClient.Connect(HostIp, HostPort);
            ConnectedFlag = true;
            RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Connect = true;
        }
    }

    public void DisconnectUdp()
    {
        if (ConnectedFlag)
        {
            UdpClient.Close();
            ConnectedFlag = false;
            Destroy(this.gameObject);
        }
    }

    private void CommandHandler(string command)
    {
        Debug.Log(command);
        if (command.StartsWith("TIME"))
        {
            if (InGameMode)
            {
                onTimeReceived(command);
            }
            else
            {
                InGameMode = true;
                SceneManager.LoadScene("Game");
            }
        }
        else if (command.StartsWith("BALLPOS"))
        {
            if (InGameMode)
            {
                onBallPosition(command);
            }
        }
        else if (command.StartsWith("BALLDIR"))
        {
            if (InGameMode)
            {
                onBallReleased(command);
            }
        }
        else if (command.StartsWith("BALLCOL"))
        {
            if (InGameMode)
            {
                onBallCollision(command);
            }
        }
        else if (command.StartsWith("BALLRESET"))
        {
            if (InGameMode)
            {
                onBallReset(command);
            }
        }
        else if (command.StartsWith("BALLCU"))
        {
            if (InGameMode)
            {
                onBallCurrentPosition(command);
            }
        }
    }
}
