using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BullettimeController : MonoBehaviour
{
    public Image bullettimeBar;
    public float current_bt;
    public float max_bt;

    public void bullettimeusage(int btusage)
    {
        current_bt = current_bt - btusage;
        bullettimeBar.fillAmount = current_bt / max_bt;
    }

    public void bullettimeregain(int btregain)
    {
        current_bt = current_bt + btregain;
        bullettimeBar.fillAmount = current_bt / max_bt;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bullettimeusage(1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            bullettimeregain(1);
        }
    }
}