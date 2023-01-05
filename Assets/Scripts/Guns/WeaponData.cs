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
    [SerializeField] SoundCentral.SoundTypes magSound;
    [SerializeField] SoundCentral.SoundTypes chargeSoundd;

    [SerializeField] int pumpTume;
    public int PumpTime { get => pumpTume; }
    public AmmoSC AmmoSource { get => ammoSource; }
    public SoundCentral.SoundTypes ShootSound { get => shootSound;}
    public SoundCentral.SoundTypes MagSound { get => magSound;  }
    public SoundCentral.SoundTypes ChargeSound { get => chargeSoundd;  }
}
