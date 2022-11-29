using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ShootBox : MonoBehaviour
{
    public readonly static BulletTag PassTag = null;
    [SerializeField] BulletTag bTag;
    // UnityEngine<Bullet> 
    [SerializeField] IShootable shootable;
    Collider collider;

    public IShootable Shootable { get => shootable;}

    public IShootable GetShootable()
    {
        return shootable;
    }
    public static ShootBox Create(BulletTag tag, IShootable shootable, Collider coll)
    {
       ShootBox b = coll.gameObject.AddComponent<ShootBox>();
        b.Init(tag, shootable, coll);

        return b;
    }
    
     void Init(BulletTag tag, IShootable shootable, Collider coll)
    {
        bTag = tag;
        collider = coll;
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