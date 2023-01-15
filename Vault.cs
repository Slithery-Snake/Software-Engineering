using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    public float timeLeft;
    public bool timerOn = false;

    public Text TimerTxt;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerOn)
        {
            if(timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                Debug.Log("Timer done");
                timeLeft = 0;
                timerOn = false;
                open();
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime++;

        float min = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.txt = string.Format("(0:00) : (1:00) ", min, sec);
    }

    void open()
    {

    }
}
