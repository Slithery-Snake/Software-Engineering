using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class DeathMenu : MonoBehaviour
{
    [SerializeField] Button retry;
    [SerializeField] Button menu;
   
    public event UnityAction Retry
    {
        add { retry.onClick.AddListener(value); }
        remove { retry.onClick.RemoveListener(value); }

    }
    public event UnityAction Menu
    {
        add { menu.onClick.AddListener(value); }
        remove { menu.onClick.RemoveListener(value); }

    }
}
