using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    public static event UnityAction Play;
    public static event UnityAction Quit;

    private void Awake()
    {

        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        
    }
     void PlayGame ()
    {
        Play?.Invoke();
    }

     void QuitGame ()
    {

       Quit?.Invoke();
    }
}