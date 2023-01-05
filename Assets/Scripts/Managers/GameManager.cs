using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

using System;
[System.Serializable]

public static class Constants
{

    public static readonly int playerMask = 6;
    public static readonly int enemyMask = 7;
    public static readonly int playerCamIgnoremask = 8;
    public static readonly int bulletMask = 9;
    public static readonly int environment = 3;
    public static readonly int managerScene = 0;
    public static readonly int menuScene = 1;
    public static readonly int levelStartScene = 2;
    public static readonly int levelEndScene = 11;
}


public class GameManager : MonoBehaviour
{ int loadedScene;
    public class GameManagerState : StateManager
    {
        StatePointer<GameManagerState,FiniteState<GameManagerState>> point;
        protected GameManager manager;
        InGame inGame;
        InMenu inMenu;

        public InGame InGame1 { get => inGame;  }
        public InMenu InMenu1 { get => inMenu;  }
        public Pointer Point { get => point;  }

        public GameManagerState(GameManager manager)
        {
            this.manager = manager;
            inMenu = new InMenu(this);
            point = new StatePointer<GameManagerState, FiniteState<GameManagerState>>(inMenu, this);

        }

        public  void ChangeToStateIG( Pointer stateToChange, int i)
        {
            stateToChange.State.ExitState();
            stateToChange.State = new InGame(this, i);
            stateToChange.State.EnterState();
        }
        public class InGame: FiniteState<GameManagerState>
        {
            int i;
            Scene1 scene;
            GameManager gameManager;
          
          void DisplayDeath(int i)
            {
             DeathMenu deathMenu =   gameManager.uiManager.ToggleDeathMenu(true);
                void Retry() { gameManager.uiManager.ToggleDeathMenu(false); gameManager.Restart(i); deathMenu.Retry -= Retry; }
                void ToMenu() { manager.ChangeToState(manager.inMenu, manager.point); deathMenu.Menu -= ToMenu; }
                deathMenu.Menu += ToMenu;
                deathMenu.Retry += Retry;
                
                
            }

            public InGame(GameManagerState manager, int i) : base(manager)
            {
                this.i = i;
                PInputManager.PlayerDied += () => { DisplayDeath(i); };
                gameManager = manager.manager;
            }

            public override void EnterState()
            {
                scene = new Scene1(i, manager.manager);

            }

            public override void ExitState()
            {
                Debug.Log("exit");

                scene.Unload();
            }
        }
        public class InMenu : FiniteState<GameManagerState>
        {
            
            public InMenu(GameManagerState manager) : base(manager)
            {
                MainMenu.Play += Start;
                MainMenu.Quit += Quit;
            }
            int sceneIndex = Constants.levelStartScene;
            
            void Start()
            {
                manager.ChangeToStateIG( manager.point, sceneIndex);
            }
           void Quit()
            {
                Application.Quit();
            }
            public override void EnterState()
            {
                manager.manager.LoadScene(Constants.menuScene);

            }

            public override void ExitState()
            {
                manager.manager.UnloadScene(Constants.menuScene);

            }
        }
    }

    void Restart(int i )
    {
        // UnloadScene(i);
        //    unloadingOperation.completed += (AsyncOperation) => { LoadScene(i); };
        gameState.ChangeToStateIG(gameState.Point, i);
    }
    AsyncOperation loadingOperation;
    AsyncOperation unloadingOperation;
    List<AsyncOperation> unloadingOperations;
    List<AsyncOperation> loadingOperations;

    GameManagerState gameState;

    [SerializeField] BulletSpawn bulletSpawn;
    [SerializeField] ItemManager itemManager;
    [SerializeField] HumanoidManager humanoidManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] SoundCentral soundManager;
   

    private void Awake()
    {
        unloadingOperations = new List<AsyncOperation>();
        loadingOperations = new List<AsyncOperation>();
        gameState = new GameManagerState(this);
    }

    
   
    public abstract class SceneCreation {
        protected int index;
        protected GameManager manager;
        public SceneCreation(int index, GameManager manager)
        {
            this.index = index;
            this.manager = manager;
        }

        public int Index { get => index; }

        public abstract void Create();
    
    }
    public class Scene1 : SceneCreation
    {   
       public BulletSpawn bSpawn;
        public ItemManager itemManager;
        public UIManager ui;
        public HumanoidManager hu;
        public PInputManager player;
      
        public Scene1(int index, GameManager manager) : base(index, manager)
        {
            manager.LoadScene(index, Create);

           
        }
        public void Unload()
        {
            manager.UnloadScene(index);
        }
     
        public override void Create()
        {   
            bSpawn = BulletSpawn.Create(manager, manager.bulletSpawn);
            itemManager = ItemManager.CreateItemManager(manager.itemManager, bSpawn);
            hu = HumanoidManager.Create(manager.humanoidManager, itemManager);
            //   player = hu.CreatePlayer(new Vector3(0, 8, -8));
            //  hu.CreateEnemy(new Vector3(0, 8, 0));
            player = hu.CreatePlayer(new Vector3(0, 8, -8));
            hu.CreateEnemy(new Vector3(0, 8, 0));
           
            ui = UIManager.Create(manager.uiManager, player);
            itemManager.CreateGun(new Vector3(3, 3, 0), 2, true);
            itemManager.CreateAmmo(new Vector3(2, 3, 3), 21, 1000);
            itemManager.CreateGun(new Vector3(0, 3, 0), 3, true);
            itemManager.CreateAmmo(new Vector3(1, 3, 3), 22, 1000);
            itemManager.CreateItem( new Vector3(4,3,4), 100);
            SoundCentral.Create(manager.soundManager);

          
        }
    }
    void OnLevelComplete()
    {
        loadedScene++;
        gameState.ChangeToStateIG(gameState.Point, loadedScene);
    }

    void Start()
    {
        
        DontDestroyOnLoad(gameObject);  // the game managers gameobject does not destroy when loading other scenes
        //  SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneIndex));
        
     //   Scene1 scene1 = new Scene1(2, this);
    }

    public void SetActive(int sceneIndex)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }
    public void LoadScene(int sceneIndex, UnityAction onDone)
    {
        LoadScene(sceneIndex);
        if (onDone != null)
        {
            loadingOperation.completed += (AsyncOperation) => onDone();
        }

    }
    public void LoadScene(int sceneIndex)
    {
        this.loadedScene = sceneIndex;
     //   if (loadingOperation != null) { return; }
        loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        loadingOperations.Add(loadingOperation);
        loadingOperation.completed += LoadOperationDone;
    
    }

    public void UnloadScene(int sceneIndex)
    {
        unloadingOperation = SceneManager.UnloadSceneAsync(sceneIndex);
        unloadingOperations.Add(unloadingOperation);
        unloadingOperation.completed += UnloadOperationDone;

    }
    void UnloadOperationDone(AsyncOperation ao)
    {

    }
    void LoadOperationDone(AsyncOperation ao)
    {
        if (loadingOperations.Contains(loadingOperation))
        {
            loadingOperations.Remove(loadingOperation);
            if (loadingOperations.Count == 0)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadedScene));
            }
        }

    }


  

    
    public void StartGame()
    {
        //LoadScene(0);
    }


    void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
    public void Quit()
    {
        Debug.Log("to the menu");
    }
  


}