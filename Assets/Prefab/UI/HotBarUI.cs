using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class HotBarUI : MonoBehaviour
{

    [SerializeField] string defaultName;
    [SerializeField] TextMeshProUGUI text;
    
    public void SetText(string str)
    {
        text.text = str;
    }
    public void SetEmpty()
    {
        text.text = defaultName;
    }
}
