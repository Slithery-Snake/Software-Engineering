using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "HotBarItems", menuName = "HotBarItemData")]
public class HotBarItemSC : ItemSC
{

    [SerializeField] Vector3 holdLocalSpace;
    [SerializeField] string HotBarName;
    [SerializeField] Vector3 lHandPos;
    [SerializeField] Vector3 rHandPos;
    public Vector3 HoldLocalSpace { get => holdLocalSpace; }
    public string HotBarName1 { get => HotBarName;  }
    public Vector3 LHandPos { get => lHandPos;  }
    public Vector3 RHandPos { get => rHandPos;  }
}