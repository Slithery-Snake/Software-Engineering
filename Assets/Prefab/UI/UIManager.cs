using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerUI playerUIFab;
    PlayerUI playerUI;
    [SerializeField] DeathMenu deathMenuFab;
    DeathMenu deathMenu;

    public PlayerUI PlayerUI { get => playerUI; }

    void CreatePlayerUI(PInputManager h)
    {
        PInputManager.UIInfoBoard board = h.uiInfo;
        playerUI = PlayerUI.Create(playerUIFab, board, h.SC, transform);
        
    }
   public static UIManager Create(UIManager fab , PInputManager h)
    {
        UIManager ui = Instantiate(fab);
        ui.CreatePlayerUI(h);
        fab.deathMenu = Instantiate(fab.deathMenuFab);
        fab.deathMenu.gameObject.SetActive(false);
        
            
        return ui;
    }
   
    public DeathMenu ToggleDeathMenu(bool b)
    {
        deathMenu.gameObject.SetActive(b);
        Cursor.lockState = CursorLockMode.Confined;
        return deathMenu;
    } 
  
}
