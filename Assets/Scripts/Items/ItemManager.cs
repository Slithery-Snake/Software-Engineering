using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void CreateItem(Vector3 v, int id)
    {
        itemDict.TryGetValue(id, out Creatable g);
        Instantiate(g, v, Quaternion.identity);
    }
    public CollectiveGun CreateGun(Vector3 v, int id, bool chamber)
    {

        gunDict.TryGetValue(id, out CollectiveGun g);
       return CollectiveGun.CreateGun(g, bulletSpawn, v, Quaternion.identity, chamber);
    }
    public Ammo CreateAmmo(Vector3 v, int id, int count)
    {
        ammoDict.TryGetValue(id, out Ammo g);
        g.Count = count;
       return Ammo.CreateAmmo(g, count, v, Quaternion.identity);
    }
    // Start is called before the first frame update
   

    // Update is called once per frame
}
