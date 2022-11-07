using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    float defaultFixedDelta;
    float  defaultTimeScale;
    void Start()
    {
        defaultFixedDelta = Time.fixedDeltaTime;
        defaultTimeScale = Time.timeScale;
    }
    bool slow = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            slow = !slow;
            if (slow)
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = defaultFixedDelta * Time.timeScale;
            } else
            {
                Time.fixedDeltaTime = defaultFixedDelta;
                Time.timeScale = defaultTimeScale;
            }
        }
    }
}
