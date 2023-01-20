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
 
}
