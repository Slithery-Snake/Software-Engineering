using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ItemManager : MonoBehaviour
{
    //[SerializeField] List<HotBarItem>  hotBarItems;
    [SerializeField] List<CollectiveGun> guns;
    //  Dictionary<int, HotBarItem> hotBarDict;
    Dictionary<int, CollectiveGun> gunDict;
    [SerializeField] List<Ammo> ammo;
    Dictionary<int, Ammo> ammoDict;
    [SerializeField] List<Creatable> generalItem;
    Dictionary<int, Creatable> itemDict;

    BulletSpawn bulletSpawn;
    public static ItemManager CreateItemManager(ItemManager prefab, BulletSpawn bulletSpawn)
    {
        ItemManager r = Instantiate(prefab);
        
        r.bulletSpawn = bulletSpawn;
        r.Init();
        return r;
    }

    private void Init()
    {
        // hotBarDict = new Dictionary<int, HotBarItem>();
        gunDict = new Dictionary<int, CollectiveGun>();
        for (int i = 0; i < guns.Count; i++)
        {
            CollectiveGun item = guns[i];
            gunDict.Add(item.ItemID, item);
        }
        ammoDict = new Dictionary<int, Ammo>();
        for (int i = 0; i < ammo.Count; i++)
        {
            Ammo item = ammo[i];
            ammoDict.Add(item.ItemID, item);
        }
        itemDict = new Dictionary<int, Creatable>();
        for (int i = 0; i < generalItem.Count; i++)
        {
            Creatable item = generalItem[i];
            itemDict.Add(item.ItemID, item);
        }
    }
    public void CreateItem(ItemStruct e)
    {
        itemDict.TryGetValue(e.id, out Creatable g);
        Instantiate(g, e.v, Quaternion.identity);
    }
    [Serializable]
    public struct ItemStruct
    {
       public Vector3 v;
       public int id;
    }
    [Serializable]
    public struct GunStruct
    {
        public Vector3 v;
        public int id;
        public bool chamber;

        public GunStruct(Vector3 v, int id, bool chamber)
        {
            this.v = v;
            this.id = id;
            this.chamber = chamber;
        }
    }
    [Serializable]
    public struct AmmoStruct {
        public Vector3 v;
        public int id;
        public int count;
        public bool inf;

        public AmmoStruct(Vector3 v, int id, int count, bool inf)
        {
            this.v = v;
            this.id = id;
            this.count = count;
            this.inf = inf;
        }
    }

    public CollectiveGun CreateGun(GunStruct e)
    {

        gunDict.TryGetValue(e.id, out CollectiveGun g);
       return CollectiveGun.CreateGun(g, bulletSpawn, e.v, Quaternion.identity, e.chamber);
    }
    public Ammo CreateAmmo(AmmoStruct e)
    {
        ammoDict.TryGetValue(e.id, out Ammo g);
     ///   g.SetCount(count);
       return Ammo.CreateAmmo(g,e. count, e.v, Quaternion.identity, e.inf);
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
}
