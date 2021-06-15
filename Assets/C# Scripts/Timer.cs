using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    void Start()
    {
        GameObject.Find("UdpClientHandler").GetComponent<UdpClientHandler>().onTimeReceived += DisplayTime;
    }
    void Update()
    {
        
    }

    void DisplayTime(string timeToDisplay)
    {
        timeText.text = timeToDisplay.Split()[1];
    }
}