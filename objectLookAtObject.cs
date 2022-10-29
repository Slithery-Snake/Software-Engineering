using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectLookAtObject : MonoBehaviour
{
    public Transform Target;
    public float Speed = 1f;


    private Coroutine = LookCoroutine;

    public void StartRotating()
    {
        if(LookCoroutine != null) 
        {
            StopCoroutine(LookCoroutine);
        }

        lookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt()
    {
        Quaternion lookRotation = Quaternion.lookRotation(Target.position - transform.position);
        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += time.deltaTime * Speed;
            yeild return null;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "Look At"))
        {
            StartRotating();
        }

    }
}
