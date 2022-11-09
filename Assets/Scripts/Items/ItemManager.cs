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


    BulletSpawn bulletSpawn;
    public static ItemManager CreateItemManager(ItemManager prefab, BulletSpawn bulletSpawn)
    {
        ItemManager r = Instantiate(prefab);
        
        r.bulletSpawn = bulletSpawn;
        return r;
    }

    private void Awake()
    {
        // hotBarDict = new Dictionary<int, HotBarItem>();
        gunDict = new Dictionary<int, CollectiveGun>();
        for (int i = 0; i < guns.Count; i++)
        {
            CollectiveGun item = guns[i];
            gunDict.Add(item.WeaponData.ItemID, item);
        }
        ammoDict = new Dictionary<int, Ammo>();
        for (int i = 0; i < ammo.Count; i++)
        {
            Ammo item = ammo[i];
            ammoDict.Add(item.ItemID, item);
        }
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
    void Start()
    {
        CollectiveGun.CreateGun(guns[0], bulletSpawn, new Vector3(3, 3, 0), Quaternion.identity, true);

        Ammo.CreateAmmo(ammo[0], 100, new Vector3(2, 3, 0), Quaternion.identity);
    }

    // Update is called once per frame
}
