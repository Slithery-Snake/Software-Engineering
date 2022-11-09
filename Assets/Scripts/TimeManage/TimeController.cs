using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeController : MonoBehaviour
{
    float defaultFixedDelta;
    public static float PlayerDelta = 1;
    public static float Slow = 1f;
    public class TimeHandler
    {   public TimeHandler (UnityAction<float> change, UnityAction def)
        {

        }
        UnityAction<float> requestChange;
        UnityAction requestNomral;
        public void RequestChange(float d) { requestChange(d); }
        public void RequestNormal() { RequestNormal(); }
    }

public static TimeController CreateController()
    {
        GameObject obj = new GameObject();
       TimeController tC =  obj.AddComponent<TimeController>();
        return tC;
    }
    void ChangeTimeScale (float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = defaultFixedDelta * Time.timeScale;
    }
    public void ReturnHandler()
    {
        TimeHandler hi = new TimeHandler(ChangeTimeScale, ReturnNormalTime);
       
    }
    void ReturnNormalTime()
    {
        ChangeTimeScale(1);
    }
    void Start()
    {
        
    }
    private void Awake()
    {
        defaultFixedDelta = Time.fixedDeltaTime;
 
    }


}
