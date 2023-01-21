using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnColEnterEvent : MonoBehaviour
{
    public event UnityAction<Collider> TriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke(other);
    }
}