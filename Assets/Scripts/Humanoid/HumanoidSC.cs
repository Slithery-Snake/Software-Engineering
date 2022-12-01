using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "humanoidSC", menuName = "HumanoidSC")]
public class HumanoidSC : ScriptableObject
{
    [SerializeField] protected float health;

    public float Health { get => health;  }
}
