using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : Poolable<Bullet>
{

    BulletTag currentTag;
    [SerializeField]BulletSC sC;
    BulletSpawn.BulletPool pool;
    [SerializeField]Rigidbody rg;
    [SerializeField] TrailRenderer trailRender;
    public BulletSC SC { get => sC;}
    public Rigidbody Rg { get => rg; set => rg = value; }
    
    public void Shoot(Vector3 worldPos, Vector3 direction, BulletTag sourceTag)
    {
        rg.velocity = Vector3.zero;
    
        currentTag = sourceTag;
        Activate();

        transform.position = worldPos;      
            transform.rotation = Quaternion.LookRotation(direction);
        rg.AddForce(direction.normalized * SC.ForceMagnitude, ForceMode.Impulse);

    }
    public static Bullet CreateBullet(BulletSC sC, BulletSpawn.BulletPool pool, Bullet prefab)
    {
        Bullet bul = Instantiate(prefab);
        bul.InitializeItem(sC, pool);
        return bul;
    }
     
    void InitializeItem(BulletSC sC, BulletSpawn.BulletPool pool)
    {
        this.sC = sC;
        this.pool = pool;
      
       
    }
  
    public static void Ignore(Bullet B)
    {
        return;
    }
    public static void Collided(Bullet b)
    {

        b.RecycleProcess(b);
    }
    public static void Hit( IShootable shootable, Bullet B)
    {
        
        shootable.ShotAt(B.SC);
        B.RecycleProcess(B);
    }
    private void OnCollisionEnter(Collision collision)
    {

        Collided(this);
    }

    private void OnTriggerEnter(Collider other) 
    {
       
        ShootBox shot = other.GetComponent<ShootBox>();
        if (shot != null)
        {
            BulletTag shotTag = shot.GetTag();
            if(shotTag == null)
            {
                Ignore(this);
                return;
            }
            UnityAction<IShootable, Bullet> temp;
            
            currentTag.RequestTagAction(shotTag, out temp);
            if(temp !=null)
            {
                temp(shot.Shootable, this);
            } else
            {
                Collided(this);
            }
            
        }
        else
        {
            Collided(this);
        }
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        
    }
    public void Deactiveate()
    {
        rg.velocity = Vector3.zero;
        gameObject.SetActive(false);    
        trailRender.Clear();

    }
    protected override void RecycleProcess(Bullet t)
    {
        Recycled();
        Deactiveate();
    }

 
    
}
