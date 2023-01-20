using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public  class HotBarItem : MonoBehaviour
{
   
   
    [SerializeField] public event UnityAction use1;
    [SerializeField] public event UnityAction use2;
    [SerializeField] public event UnityAction use1Up;
    [SerializeField] public event UnityAction use2Up;
    Creatable ogItem;
   
    //  [SerializeField] UnityAction rDown;
    // [SerializeField] UnityAction rUp;
    public event UnityAction Destroyed;

    protected HotBarItemSC sc;
    protected Interactable interact;
    public HotBarItemSC ItemData { get => sc; }
    public Creatable OgItem { get => ogItem; }
    public Interactable Interact { get => interact; }
    private void OnDestroy()
    {
        Destroyed?.Invoke();
    }
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

    public static HotBarItem CreateHotBar(UnityAction use1, UnityAction use2, UnityAction up1, UnityAction up2, HotBarItemSC itemData, GameObject objectToAdd, Creatable c, Interactable inter)
    {
        HotBarItem h = objectToAdd.AddComponent<HotBarItem>();

        h.use1 = use1;
        h.use2 = use2;
        h.use1Up = up1;
        h.use2Up = up2;
        h.sc = itemData;
        h.interact = inter;
        h.ogItem = c;
        
        return h;

    }

 





    // Update is called once per frame

}
