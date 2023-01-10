using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShottyData", menuName = "ShottySc")]

public class ShottySC : ScriptableObject
{

    [SerializeField] int pellets;
    
    public int Pellets { get => pellets;  }
  
   
}
