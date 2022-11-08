using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
    public Image healthBar;
    public float current_health;
    public float max_health;

    public void takedamage(int dmg)
    {
        current_health = current_health - dmg;
        healthBar.fillAmount = current_health / max_health;
    }

    public void heal (int heal)
    {
        current_health = current_health + heal;
        healthBar.fillAmount = current_health / max_health;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            takedamage(20);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            heal(20);
        }
    }
}
