using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "humanoidSC", menuName = "HumanoidSC")]
public class HumanoidSC : ScriptableObject
{
    [SerializeField] protected float health;

    public float Health { get => health; }
    public MeleeStats Light { get => light;  }
    public MeleeStats Heavy { get => heavy; }

   [SerializeField] protected MeleeStats light;
    [SerializeField] protected MeleeStats heavy;


}
[Serializable]
public struct MeleeStats
{
    public int dmg;
    public float length;
    public float coolDown;
    public float windUp;

}