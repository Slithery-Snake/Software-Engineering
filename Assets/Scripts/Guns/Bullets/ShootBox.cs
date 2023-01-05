using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ShootBox : MonoBehaviour
{
    public readonly static BulletTag PassTag = null;
    [SerializeField] BulletTag bTag;
    // UnityEngine<Bullet> 
     IShootable shootable;
   
    public IShootable Shootable { get => shootable;}
    StatusEffect.StatusEffectManager.IStatusEeffectable statusEffectManager;
    public StatusEffect.StatusEffectManager.IStatusEeffectable Status { get => statusEffectManager; }

    public IShootable GetShootable()
    {
        return shootable;
    }
    public static ShootBox Create(BulletTag tag, IShootable shootable, Collider coll, StatusEffect.StatusEffectManager.IStatusEeffectable statusEffectManager)
    {
       ShootBox b = coll.gameObject.AddComponent<ShootBox>();
        b.statusEffectManager = statusEffectManager;
        b.Init(tag, shootable);

        return b;
    }
    
     void Init(BulletTag tag, IShootable shootable)
    {
        bTag = tag;
        this.shootable = shootable;
        if(shootable ==  null)
        {
            this.shootable = new ShootableNull();
        } 
    }
    public BulletTag GetTag()
    { 
        return bTag;
    }
    
}

class ShootableNull : IShootable
{
    void IShootable.ShotAt(BulletSC bullet)
    {
        Debug.LogError("No Shootable On Box");
    }
}
public interface IShootable
{


    public void ShotAt(BulletSC bullet);
}