using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public interface Iinteractable
{


    public void Interacted(SourceProvider source);
    


}
public interface SourceProvider
{
    StatusEffect.StatusEffectManager Status { get; }
    Inventory Inventory { get;  }
}
    public class Interactable : MonoBehaviour, Iinteractable
{
    UnityAction<SourceProvider> func;
    bool enable;
    
    public static Interactable Create(GameObject obj, Iinteractable func )
    {
         Interactable r = obj.AddComponent<Interactable>();
        r.func = func.Interacted;
        return r;
    }
    public void OnDisable()
    {
        enable = false;   
    }
    public void OnEnable()
    {
        enable = true;   
    }
    public void Interacted(SourceProvider source)
    {
        if(enable && func !=null) { func(source); }
    }
}

