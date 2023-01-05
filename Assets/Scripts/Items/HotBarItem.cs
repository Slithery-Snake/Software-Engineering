using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public  class HotBarItem : MonoBehaviour
{
   
   
    [SerializeField] protected UnityAction use1;
    [SerializeField] protected UnityAction use2;
    [SerializeField] protected UnityAction use1Up;
    [SerializeField] protected UnityAction use2Up;
    Creatable ogItem;
    //  [SerializeField] UnityAction rDown;
    // [SerializeField] UnityAction rUp;

    protected HotBarItemSC sc;

    public HotBarItemSC ItemData { get => sc; }
    public Creatable OgItem { get => ogItem; }

    public void Use1()
    {
        if(use1 !=null)
        {
            use1();
        }
    }
    public void Use2( )
    {
        if (use2 != null)
        {
            use2();
        }
    }
    public void Use1Up() {
        if (use1Up != null)
        {
            use1Up();
        }
    }
    public void Use2Up() {
        if (use2Up != null)
        {
            use2Up();
        }
    }
    Rigidbody rg;
    //  public UnityAction RDown { get => rDown; }
    //   public UnityAction RUp { get => rUp;  }

    public static HotBarItem CreateHotBar(UnityAction use1, UnityAction use2, UnityAction up1, UnityAction up2, HotBarItemSC itemData, GameObject objectToAdd, Creatable c)
    {
        HotBarItem h = objectToAdd.AddComponent<HotBarItem>();

        h.use1 = use1;
        h.use2 = use2;
        h.use1Up = up1;
        h.use2Up = up2;
        h.sc = itemData;
        h.ogItem = c;
        
        return h;

    }

 





    // Update is called once per frame

}
