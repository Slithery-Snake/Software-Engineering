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

    public IShootable Shootable { get => shootable;}

    public IShootable GetShootable()
    {
        return shootable;
    }
    private void Awake()
    {
      
    }
    public static ShootBox CreateBox(BulletTag tag, IShootable shoot, GameObject obj)
    {
        ShootBox r = obj.AddComponent<ShootBox>();
        r.shootable = shoot;
        r.Init(tag);
        return r;
    }
    private void Init(BulletTag tag)
    {
        bTag = tag;   
        if(shootable ==  null)
        {
            shootable = new ShootableNull();
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