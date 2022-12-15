using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShottyData", menuName = "ShottySc")]

public class ShottySC : ScriptableObject
{

    [SerializeField] int pellets;
    [SerializeField] int radius;
    [SerializeField] int pumpTume;

    public int Pellets { get => pellets;  }
    public int Radius { get => radius; }
    public int PumpTime { get => pumpTume; }
}
