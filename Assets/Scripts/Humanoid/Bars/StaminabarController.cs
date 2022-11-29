using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminabarController : MonoBehaviour
{
    public Image staminaBar;
    public float current_stam;
    public float max_stam;

    public void stamdamage(int stamdmg)
    {
        current_stam = current_stam - stamdmg;
        staminaBar.fillAmount = current_stam / max_stam;
    }

    public void stamheal (int stamheal)
    {
        current_stam = current_stam + stamheal;
        staminaBar.fillAmount = current_stam / max_stam;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            stamdamage(20);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            stamheal(20);
        }
    }
}
