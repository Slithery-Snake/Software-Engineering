using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HotBarItem : Item<HotBarItemSC>, Iinteractable
{
   
    [SerializeField] UnityAction use1;
    [SerializeField] UnityAction use2;
    [SerializeField] UnityAction use1Up;
    [SerializeField] UnityAction use2Up;
  //  [SerializeField] UnityAction rDown;
   // [SerializeField] UnityAction rUp;
    public UnityAction Use1 { get => use1; }
    public UnityAction Use2 { get => use2; }
    public UnityAction Use1Up { get => use1Up; }
    public UnityAction Use2Up { get => use2Up; }
  //  public UnityAction RDown { get => rDown; }
 //   public UnityAction RUp { get => rUp;  }

   
    public static HotBarItem CreateHotBar(UnityAction use1, UnityAction use2, UnityAction up1, UnityAction up2, HotBarItemSC r, GameObject objectToAdd)
    {
        HotBarItem h = objectToAdd.AddComponent<HotBarItem>();
        

        h.use1 = use1;
        h.use2 = use2;
        h.use1Up = up1;
        h.use2Up = up2;
        h.itemData = r;
        
        return h;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interacted(PInputManager source)
    {
     
        source.Inventory.AddHotBarItem(this);
    }

    // Update is called once per frame

}
