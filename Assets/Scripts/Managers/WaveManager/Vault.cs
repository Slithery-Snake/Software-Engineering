using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour, Iinteractable
{
    public float timeLeft;
    public bool timerOn = false;
    WaitForSeconds wait = new WaitForSeconds(1);

    //public Text TimerTxt;

    // Start is called before the first frame update
    void Awake()
    {
        timerOn = true;
    }

    // Update is called once per frame
    IEnumerator Tick()
    {
        while (true)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                // UpdateTimer(timeLeft);
            }
            else
            {
                Debug.Log("Timer done");
                timeLeft = 0;
                timerOn = false;
                Open();
                break;
            }
            yield return wait;
        }
      
    }

    void UpdateTimer(float currentTime)
    {
        currentTime++;

        float min = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

    //    TimerTxt.txt = string.Format("(0:00) : (1:00) ", min, sec);
    }

    void Open()
    {

    }
    void StartOpening()
    {

    }
    public void Interacted(SourceProvider source)
    {
      
    }
}