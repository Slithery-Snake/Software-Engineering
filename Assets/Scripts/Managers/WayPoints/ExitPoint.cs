using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider))]

public class ExitPoint : MonoBehaviour
{
   public BoxCollider hitBox;
    public UnityAction InExit;
    public UnityAction OutExit;
    private void Awake()
    {
        hitBox = GetComponent<BoxCollider>();
        hitBox.isTrigger = true;
        GameManager.SpawnData.exitPoint = this;
    }
     void OnTriggerEnter(Collider other)
    {
        InExit?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        OutExit?.Invoke();   
    }
}
