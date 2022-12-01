using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponItem")]

public class WeaponData : HotBarItemSC
{
    public int magSize;
    public int reloadTime;
    public float weaponCDTime;
    [SerializeField] AmmoSC ammoSource;

    public AmmoSC AmmoSource { get => ammoSource; }
}
