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
    [SerializeField] SoundCentral.SoundTypes shootSound; 
    public AmmoSC AmmoSource { get => ammoSource; }
    public SoundCentral.SoundTypes ShootSound { get => shootSound;}
}
