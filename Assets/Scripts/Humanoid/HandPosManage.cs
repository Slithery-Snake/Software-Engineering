using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosManage : StateManagerComponent
{                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Vector3 rDefault;
    Vector3 lDefault;
    
    public HandPosManage(MonoCalls.MonoAcessors manager, Transform rHand, Transform lHand) : base(manager)
    {
        this.rHand = rHand;
        this.lHand = lHand;
        rDefault = rHand.localPosition;
        lDefault = lHand.localPosition;
    }
    Transform rHand;
    Transform lHand;

    public void SetRHand(Vector3 v)
    {
        rHand.localPosition = v;
    }
    public void SetLHand(Vector3 v)
    {
        lHand.localPosition = v;

    }
    public void SetDefault()
    {
        rHand.localPosition = rDefault;
        lHand.localPosition = lDefault;
    }
}
