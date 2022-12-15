using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerUI playerUIFab;
    PlayerUI playerUI;
     void CreatePlayerUI(PInputManager h)
    {
        PInputManager.UIInfoBoard board = h.uiInfo;
        playerUI = PlayerUI.Create(playerUIFab, board, h.SC);
        
    }
   public static UIManager Create(UIManager fab , PInputManager h)
    {
        UIManager ui = Instantiate(fab);
        ui.CreatePlayerUI(h);
        
            
        return ui;
    }
   

  
}
