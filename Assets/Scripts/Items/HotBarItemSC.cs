using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HotBarItemSC : ItemSC
{

    [SerializeField] Vector3 holdLocalSpace;
 

    public Vector3 HoldLocalSpace { get => holdLocalSpace; }
  
}