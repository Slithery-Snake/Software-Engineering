using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    float defaultFixedDelta;
    float  defaultTimeScale;
    float timeSlow = 0.1f;
    float playerSlowAdvantage = 5;
    void Start()
    {
        defaultFixedDelta = Time.fixedDeltaTime;
        defaultTimeScale = Time.timeScale;
    }
    bool slow = false;
    // Update is called once per frame
    void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.G))
        {
            slow = !slow;
            if (slow)
            {
                TimeController.Slow = timeSlow;
                Time.timeScale = timeSlow;
                Time.fixedDeltaTime = defaultFixedDelta * Time.timeScale;
                TimeController.PlayerDelta = playerSlowAdvantage;
            } else
            {
                TimeController.Slow = 1;
                Time.fixedDeltaTime = defaultFixedDelta;
                Time.timeScale = defaultTimeScale;
                TimeController.PlayerDelta = 1;
            }
        }*/
    }
}
