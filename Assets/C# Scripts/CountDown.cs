using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CountDown : MonoBehaviour
{

    public GameObject AnimatedCount;
    public bool ispasued = false;
    int flagstart = 0;
    public int countdownTime;
    public Text countdownDisplay;
    // Start is called before the first frame update


    public void Pasuegame()
    {
        /*
        AnimatedCount.SetActive(true);
        Time.timeScale = 0f;

        ispasued = true;
        */
    }
    public void Resumegame()
    {
        /*
        AnimatedCount.SetActive(false);
        Time.timeScale = 1f;
        ispasued = false;
        */
    }

    private void Start()
    {
        /*
        Pasuegame();
        StartCoroutine(CountdownToStart());
        */
    }
    /*
       IEnumerator CountdownToStart()
       {

           if (flagstart == 0)
           {
               while(countdownTime > 0)
               {
                   countdownDisplay.text = countdownTime.ToString();
                   yield return new WaitForSeconds(1f);
                   countdownTime--;

               }
               countdownDisplay.text = "Go!";
               Resumegame();
           }



       }
    */

    // Update is called once per frame
    void Update()
    {
      
        
  
    }
  


}
