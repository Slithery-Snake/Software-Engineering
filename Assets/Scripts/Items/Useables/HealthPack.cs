using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class HealthPack : Item<HotBarItemSC>, Iinteractable
{
    HotBarItem hbar;
    StatusEffect.StatusEffectManager status;
    Interactable interact;
    static MonoCall healed = new MonoCall();

    public static IMonoCall Healed { get => healed; }

    public void Interacted(SourceProvider source)
    {
        hbar = HotBarItem.CreateHotBar(Heal, null, null, null, itemData, gameObject, this, interact);
        source.Inventory.TryAddHotBar(hbar);
        status = source.Status;
        interact.enabled = false;
       
    }
   void Awake()
    {
        interact = Interactable.Create(gameObject, this);
    }
    void Heal()
    {
        SoundCentral.Instance.Invoke(transform.position, SoundCentral.SoundTypes.Heal);
        healed.Call();
        status?.AddStatusEffect(new StatusEffect.StatusEffectManager.HealthApply(), null);
        Destroy(gameObject);
        

    }

   
}
