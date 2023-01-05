using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HealthPack : Item<HotBarItemSC>, Iinteractable
{
    HotBarItem hbar;
    StatusEffect.StatusEffectManager status;

    public void Interacted(PInputManager source)
    {
        Debug.Log("health kitPicked");
        hbar = HotBarItem.CreateHotBar(Heal, null, null, null, itemData, gameObject, this);
        source.Inventory.TryAddHotBar(hbar);
        status = source.StatusEffectManager;
    }
   
    void Heal()
    {
        
        status?.AddStatusEffect(new StatusEffect.StatusEffectManager.HealthApply());
        Destroy(gameObject);

    }

   
}
