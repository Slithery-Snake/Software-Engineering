using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using System;
using System.Threading;
using System.Threading.Tasks;
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
   
    public static class SpawnData
    {
        public static List<AmmoSpawn> ammo = new List<AmmoSpawn>();
        public static List<GunSpawn> guns = new List<GunSpawn>();
        public static List<EnemySpawn> enemy = new List<EnemySpawn>();

        public static List<ItemSpawn> item = new List<ItemSpawn>();
        public static PlayerSpawn whereSpawn;
        public static ExitPoint exitPoint;
        
        public static void ClearAll()
        {
           ammo.Clear();
            guns.Clear();
           enemy.Clear();
            item.Clear();
        }

    }
    private void OnDestroy()
    {
        gameState.Dispose();
       // gameState = null;
        
        SpawnData.ClearAll();
        Debug.Log("game manager disposed");
        
    }
   
    
    public class GameManagerState : StateManager, IDisposable
    {
     
        List<SceneCreation> levelScenes;
        int levelIndex = 0;
        StatePointer<GameManagerState,FiniteState<GameManagerState>> point;
        protected GameManager manager;
        InGame inGame;
        InMenu inMenu;
        
        public InGame InGame1 { get => inGame;  }
        public InMenu InMenu1 { get => inMenu;  }
        public Pointer Point { get => point;  }
       public void Dispose()
        {
            levelScenes.Clear();
            inGame = null;
            inMenu = null;
            point.State = null;
        }
        public GameManagerState(GameManager manager)
        {
            this.manager = manager;
            inMenu = new InMenu(this);
            point = new StatePointer<GameManagerState, FiniteState<GameManagerState>>(inMenu, this);
            levelScenes = new List<SceneCreation>();
            levelScenes.Add(new Scene1(manager));

        }

        public  void ChangeToStateIG( Pointer stateToChange, int i)
        {
            stateToChange.State.ExitState();

            stateToChange.State = new InGame(this, i);
            stateToChange.State.EnterState();
        }
        public class InGame: FiniteState<GameManagerState>
        {
            int sceneIndex;
            SceneCreation scene;
            GameManager gameManager;

            DeathMenu deathMenu;
          void DisplayDeath()
            {
              deathMenu =   gameManager.uiManager.ToggleDeathMenu(true);
              
                deathMenu.Menu += ToMenu;
                deathMenu.Retry += Retry;
                
                
            }
            void Retry() { gameManager.uiManager.ToggleDeathMenu(false); gameManager.Restart(sceneIndex); deathMenu.Retry -= Retry; }
            void ToMenu() { manager.ChangeToState(manager.inMenu, manager.point); deathMenu.Menu -= ToMenu; }

            void OnLevelComplete()
            {
                Debug.Log("LEVEL COMPLETE, YOU ARE THE BIGEST BIRD");
                manager.levelIndex++;
                manager.ChangeToStateIG(manager.point, sceneIndex++);
            }
            public InGame(GameManagerState manager, int i) : base(manager)
            {
                this.sceneIndex = i + manager.levelIndex;
                PInputManager.PlayerDied += Death;
                gameManager = manager.manager;

            }
            void Death() { DisplayDeath(); }

            public override void EnterState()
            {
                manager.levelScenes[manager.levelIndex].StartLoadAndCreate(sceneIndex);

                scene = manager.levelScenes[manager.levelIndex];
                    scene.LevelDone += OnLevelComplete;

            }

            public override void ExitState()
            {
                deathMenu.Menu -= ToMenu;
                deathMenu.Retry -= Retry;

                PInputManager.PlayerDied -= Death;
                scene.LevelDone -= OnLevelComplete;
                scene.Unload();               
                scene.Dispose();
                SpawnData.ClearAll();

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
                manager.levelIndex = 0;
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
                MainMenu.Play -= Start;
                MainMenu.Quit -= Quit;
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

    
   
    public abstract class SceneCreation: IDisposable
    {
        protected int index;
        protected GameManager manager;
        public BulletSpawn bSpawn;
        public ItemManager itemManager;
        public UIManager ui;
        public HumanoidManager hu;
        public PInputManager player;
        static TextMeshProUGUI text;
        public static TextMeshProUGUI Text { get => text; }
        public SceneCreation(GameManager manager)
        {
            this.manager = manager;
        }
        protected abstract void AdditionalCreation();
        public event UnityAction LevelDone;
        protected void InvokeLevelDone()
        {
            LevelDone?.Invoke();
        }
        public int Index { get => index; }
        public void StartLoadAndCreate(int index)
        {
            this.index = index;

            manager.LoadScene(index, Create);
        }
        public  void Create()
        {
            
            bSpawn = BulletSpawn.Create(manager, manager.bulletSpawn);
            itemManager = ItemManager.CreateItemManager(manager.itemManager, bSpawn);
            hu = HumanoidManager.Create(manager.humanoidManager, itemManager);
            //   player = hu.CreatePlayer(new Vector3(0, 8, -8));
            //  hu.CreateEnemy(new Vector3(0, 8, 0));
            player = hu.CreatePlayer(SpawnData.whereSpawn.transform.position);

            ui = UIManager.Create(manager.uiManager, player);

            SoundCentral.Create(manager.soundManager);
            text = ui.PlayerUI.MessageText;
            AdditionalCreation();

        }
        protected abstract void CleanUp();
        public void Dispose()
        {
            CleanUp();
        }
        public void Unload()
        {
            manager.UnloadScene(index);
        }
    }
    public class Scene1 : DefaultSpawnScene
    {


        static Queue<Messages> messages;
        static Messages currentMessage;

        public Scene1( GameManager manager) : base( manager)
        {
            messages = new Queue<Messages>();
        }
        void OnGunEquipped()
        {
            player.Inventory.GunEquipped.Deafen(OnGunEquipped);

            Inventory pInventory = player.Inventory;
            
            Shooting shooting = pInventory.CurrentGun.Shooting;
            messages.Enqueue(new Messages("Fire the weapon (left click)", 2000, shooting.SomeBulletShot));
            messages.Enqueue(new Messages("Now reload the weapon (press r)", 2000, shooting.Reloaded));
            messages.Enqueue(new Messages("Now fire until the weapon is empty", 2000, shooting.Empty));
            messages.Enqueue(new Messages("Now reload the weapon (press r)", 2000, shooting.Reloaded));
            messages.Enqueue(new Messages("See how it won't fire? You need to chambr the bullet (press r again)", 2000, shooting.Reloaded));
            messages.Enqueue(new Messages("Equip your medkit and use it(press number to unequip then equip medkit, then left click to use)", 2000, HealthPack.Healed));
            messages.Enqueue(new Messages("Your combat suit can't protect against everything. Use medkits when needded.", 2000));
            messages.Enqueue(new Messages("Be sure to drop things to make space in your hands (press x)", 2000, pInventory.SomeItemDropped));
            messages.Enqueue(new Messages("Good. You may lose ammunition, so let's learn about CQC", 2000));
            messages.Enqueue(new Messages("Try a strike (left click with nothing equipped)", 2000));
            messages.Enqueue(new Messages("A strike is fast and hurts others well", 2000));
            messages.Enqueue(new Messages("Try a grapple (left click then left click again", 2000));
            messages.Enqueue(new Messages("A grapple is weak but disorients others well", 2000));
            messages.Enqueue(new Messages("Good Job Agent. Let me introduce myself. I am your operator. We have been tasked with infiltrating the criminal shadow organization Leviathan.", 5000));
            messages.Enqueue(new Messages("The fact that we're attacking them has nothing to do with the fact that they are a crime organization. Rather, they have not been paying their taxes.", 5000));
            messages.Enqueue(new Messages("As an enforcer for the IRS, teach them to never mess with us, and get their tax dollars!", 5000));

        }
        protected override void AdditionalCreation()
        {
            base.AdditionalCreation();
            messages.Enqueue(new Messages("", 3000));

            messages.Enqueue(new Messages("Hello  agent, let's get straight into it", 3000));
            messages.Enqueue(new Messages("Get used to your combat suit. Try Moving around. (WASD to move, space to jump, shift to sprint)", 4000,HumanoidManager.PlayerMovedCall));
            messages.Enqueue(new Messages("Good. On your interface, you'll see a red, blue, and black bar. The red bar is your health, the blue bar is your stamina, and the black bar are your adrenaline levels (press B to toggle an adrenaline rush)", 6000, TimeSlow.TimeSlowAttempted));
            messages.Enqueue(new Messages("You aren't in a dangeorus situation for now, so nothing will happen (kill enemies to build up adrenaline)", 8000));
            
            messages.Enqueue(new Messages("Pick up those items and ammo (press E when close and looking)" , 2000, player.Inventory.SomeItemAddded));
            player.Inventory.GunEquipped.Listen(OnGunEquipped);
            messages.Enqueue(new Messages("Good now equip the gun (use number keys to eqiup)", 2000, player.Inventory.GunEquipped));
            Inventory pInventory = player.Inventory;
           










            Text.text = messages.Peek().TextToDisplay;
            currentMessage = messages.Peek();
            currentMessage.StartListening();
        }
        protected override void CleanUp()
        {
            base.CleanUp();
            currentMessage.Stop();
            
            currentMessage = null;
        
            messages.Clear();
           
           // Debug.Log("message scleared");
        }


        public class Messages
        {
          
            public string TextToDisplay;
            public int waitUntilNextDisplayMS;
            private readonly IMonoCall call;
            CancellationTokenSource source;
            public void StartListening()
            {
                if (call != null)
                {
                    call.Listen(Fulfilled);
                }
                else
                {
                    Fulfilled();
                }
            }
            async void Run(CancellationToken token)
            {try
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    
                    await CoolDown();
                    


                    if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                    TextToDisplay = "";
                    messages.Dequeue();
                    currentMessage = messages.Peek();
                    Text.text = messages.Peek().TextToDisplay;
                    
                    messages.Peek().StartListening();
                    SoundCentral.Instance.Invoke(HumanoidManager.PlayerTransform, SoundCentral.SoundTypes.Message);
                   // Debug.Log("soundRequest " + TextToDisplay);
                }
                catch(OperationCanceledException e)
                {
                    Debug.Log("sound cancelled " + e);
                }
                finally
                {
                    source?.Dispose();

                }
            }
            public void Stop()
            {
               // Debug.Log("attempt cancel "+ source);

                source?.Cancel();

            }
            async Task CoolDown()
            {
                await Task.Delay(waitUntilNextDisplayMS);

            }
            void  Fulfilled()
            {
               // Debug.Log("fulfilled");
                source = new CancellationTokenSource();
                CancellationToken t = source.Token;
                Run(t);
                call?.Deafen(Fulfilled);



            }

            public Messages(string str,int  waitUntilNextDisplayMS = 0, IMonoCall call = null)
            {
                TextToDisplay = str;
                this.waitUntilNextDisplayMS = waitUntilNextDisplayMS;
                this.call = call;
               
            }
        }
    }


    public class DefaultSpawnScene : SceneCreation
    {   
      
        int enemiesLeft;

        public int EnemiesLeft { get => enemiesLeft;  }
        

        
        public DefaultSpawnScene( GameManager manager) : base( manager)
        {


           
        }
       
        bool inExit = false;
        void InEx() { inExit = true; 
            if(enemiesLeft <= 0)
            {
                InvokeLevelDone();
                
            }
        }
        void NotInEx() { inExit = false; }
        void ReduceEnemy()
        {
            enemiesLeft--;
           
        }
     
        protected override void AdditionalCreation()
        {   
        
            for(int i = 0; i < SpawnData.enemy.Count; i ++)
            {
                EnemySpawn e = SpawnData.enemy[i];
                hu.CreateEnemy(e.transform.position, e.transform.eulerAngles.y, e.enemyPReFab);

            }
            EnemyStuff.EnemyAI.EnemyKilled += ReduceEnemy;
            enemiesLeft = SpawnData.enemy.Count;
            SpawnData.exitPoint.InExit += InEx;
            SpawnData.exitPoint.InExit += NotInEx;

            //  hu.CreateEnemy(new Vector3(3, 6, 0), 180);
            for (int i = 0; i < SpawnData.guns.Count; i++)
            {
                GunSpawn e = SpawnData.guns[i];
                itemManager.CreateGun(e.transform.position, e.id, e.chamber);

            }
            for (int i = 0; i < SpawnData.ammo.Count; i++)
            {
                AmmoSpawn e = SpawnData.ammo[i];
                itemManager.CreateAmmo(e.transform.position, e.id,e.count, e.inf);

            }
            for (int i = 0; i < SpawnData.item.Count; i++)
            {
                ItemSpawn e = SpawnData.item[i];
                itemManager.CreateItem(e.transform.position, e.ID);

            }
        
          
        }

        protected override void CleanUp()
        {   
            EnemyStuff.EnemyAI.EnemyKilled -= ReduceEnemy;
            SpawnData.exitPoint.InExit -= InEx;
            SpawnData.exitPoint.InExit -= NotInEx;
            SpawnData.ClearAll();

        }
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
        unloadingOperation.completed -= UnloadOperationDone;

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
        loadingOperation.completed -= LoadOperationDone;


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