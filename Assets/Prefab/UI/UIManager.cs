using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerUI playerUIFab;
    PlayerUI playerUI;
   public static UIManager Create(UIManager fab, PInputManager h)
    {
        UIManager ui = Instantiate(fab);
        PInputManager.UIInfoBoard board = h.uiInfo;
        ui.playerUI = PlayerUI.Create(ui.playerUIFab, board, h.SC);

        return ui;
    }
   

  
}
