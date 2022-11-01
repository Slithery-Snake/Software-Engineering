using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoData", menuName = "AmmoItem")]
public class AmmoSC : ItemSC
{
    [SerializeField] BulletSC bulletType;

    public BulletSC BulletType { get => bulletType; }
}
