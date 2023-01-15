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
        public static List<AmmoSpawn> ammo;
        public static List<GunSpawn> guns;
        public static List<EnemySpawn> enemy;
        public static List<EnemyGunner> gunners;

        public static List<ItemSpawn> item;
        public static PlayerSpawn whereSpawn;
        public static ExitPoint exitPoint;
        
        public static void ClearAll()
        {
           ammo.Clear();
            guns.Clear();
           enemy.Clear();
            item.Clear();
            gunners.Clear();
            whereSpawn = null;
            exitPoint = null;
        }

    }
    private void Awake()
    {
        
        unloadingOperations = new List<AsyncOperation>();
        loadingOperations = new List<AsyncOperation>();
        gameState = new GameManagerState(this);
           SpawnData.ammo = new List<AmmoSpawn>();
        SpawnData.gunners = new List<EnemyGunner>();
        SpawnData.guns = new List<GunSpawn>();
        SpawnData.enemy = new List<EnemySpawn>();

        SpawnData.item = new List<ItemSpawn>();
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
        int finalLevelIndex = 3 ;
        StatePointer<GameManagerState,FiniteState<GameManagerState>> point;
        protected GameManager manager;
        InGame inGame;
        InMenu inMenu;
        
        public InGame InGame1 { get => inGame;  }
        public InMenu InMenu1 { get => inMenu;  }
        public Pointer Point { get => point;  }
       public void Dispose()
        {
            foreach(SceneCreation sc in levelScenes)
            {
                sc.Dispose();
            }
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
                if (manager.levelIndex > manager.finalLevelIndex)
                {
                    manager.ChangeToState(manager.inMenu, manager.point );
                } else
                {
                    manager.ChangeToStateIG(manager.point, sceneIndex++);

                }
            }
            public InGame(GameManagerState manager, int i) : base(manager)
            {
                this.sceneIndex = i + manager.levelIndex;
                gameManager = manager.manager;

            }
            void Death() { DisplayDeath(); }

            public override void EnterState()
            {
                SceneCreation sCreation = manager.levelScenes[manager.levelIndex];
                if(sCreation == null)
                {
                    manager.ChangeToState(manager.inMenu, manager.point);
                }
                sCreation.StartLoadAndCreate(sceneIndex);

                scene = manager.levelScenes[manager.levelIndex];
                    scene.LevelDone += OnLevelComplete;
                PInputManager.PlayerDied += Death;

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
       
        

        public Scene1( GameManager manager) : base( manager)
        {
            messageManage = new MessageManage(manager);
        }
        MessageManage messageManage;
        void MessagesDone()
        {
            MessageManage.Messages.MessageEmpty -= MessagesDone;
            InvokeLevelDone();

        }
      
        void OnGunEquipped()
        {
            MessageManage.Messages.MessageEmpty += MessagesDone;

            player.Inventory.GunEquipped.Deafen(OnGunEquipped);

            Inventory pInventory = player.Inventory;
            
            Shooting shooting = pInventory.CurrentGun.Shooting;
            
            MessageManage.messages.Enqueue(new MessageManage.Messages("Fire the weapon. (left click)", 2000, shooting.SomeBulletShot));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Now reload the weapon. (press r)", 2000, shooting.Reloaded));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Now fire until the weapon is empty.", 2000, shooting.Empty));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Now reload the weapon. (press r)", 2000, shooting.Reloaded));
            MessageManage.messages.Enqueue(new MessageManage.Messages("You need to chamber the bullet because you've emptied your magazine. (press r again)", 3000, shooting.Reloaded));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Equip your medkit and use it. (press number to unequip then equip medkit, then left click to use)", 2000, HealthPack.Healed));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Your combat suit can't protect against everything. Use medkits when needed.", 4000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Be sure to drop things to make space in your hands. (equip and press x)", 3000, pInventory.SomeItemDropped));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Good. You may run out of ammunition, so let's learn about CQC.", 5000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Try a strike. (left click with nothing equipped)", 2000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("A strike is fast and hurts others well.", 4000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Try a grapple. (left click then left click again", 2000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("A grapple is weak but disorients others well.", 5000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Good Job Agent. Let me introduce myself. As you can tell, I am your operator. We have been tasked with infiltrating the criminal shadow organization known as the Agency.", 10000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("The fact that we're attacking them has nothing to do with the fact that they are a crime organization. Rather, they have not been paying their taxes.", 8000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("As an enforcer for the IRS, teach them to never mess with us, and steal their tax dollars", 7000));
            


        }
        protected override void AdditionalCreation()
        {
            base.AdditionalCreation();
            MessageManage.messages.Enqueue(new MessageManage.Messages("", 3000));

            MessageManage.messages.Enqueue(new MessageManage.Messages("Hello  agent, let's get straight into it", 3000));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Get used to your combat suit. Try Moving around. (WASD to move, space to jump, shift to sprint)", 4000,HumanoidManager.PlayerMovedCall));
            MessageManage.messages.Enqueue(new MessageManage.Messages("Good. On your interface, you'll see a red, blue, and black bar. The red bar is your health, the blue bar is your stamina, and the black bar are your adrenaline levels (press V to toggle an adrenaline rush)", 6000, TimeSlow.TimeSlowAttempted));
            MessageManage.messages.Enqueue(new MessageManage.Messages("You aren't in a dangeorus situation for now, so nothing will happen (kill enemies to build up adrenaline)", 8000));

            MessageManage.messages.Enqueue(new MessageManage.Messages("Pick up those items and ammo (press E when close and looking)" , 2000, player.Inventory.SomeItemAddded));
            player.Inventory.GunEquipped.Listen(OnGunEquipped);
            MessageManage.messages.Enqueue(new MessageManage.Messages("Good now equip the gun (use number keys to eqiup)", 2000, player.Inventory.GunEquipped));
            Inventory pInventory = player.Inventory;
           










            Text.text = MessageManage.messages.Peek().TextToDisplay;
            MessageManage.currentMessage = MessageManage.messages.Peek();
            MessageManage.currentMessage.StartListening();
        }
        protected override void CleanUp()
        {
            base.CleanUp();
            MessageManage.Messages.MessageEmpty -= MessagesDone;
            messageManage.Dispose();
            // Debug.Log("message scleared");
        }

        public class MessageManage : IDisposable
        {
            GameManager manager;

            public MessageManage(GameManager manager)
            {
                this.manager = manager;
                messages = new Queue<Messages>();
                currentMessage = null;
            }

            public static Queue<Messages> messages;
            public static Messages currentMessage;

            public void Dispose()
            {

                MessageManage.currentMessage.Stop();
               
                MessageManage.currentMessage = null;

                MessageManage.messages.Clear();
            }


            public class Messages
            {

                public string TextToDisplay;
                public int waitUntilNextDisplayMS;
                private readonly IMonoCall call;
                CancellationTokenSource source;
                public static event UnityAction MessageEmpty;
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
                {
                    try
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
                        else
                        {
                            TextToDisplay = "";
                            if (messages.Count > 0)
                            {
                                currentMessage = messages.Dequeue();

                                Text.text = currentMessage.TextToDisplay;


                                currentMessage.StartListening();
                            }
                            else
                            {
                                MessageEmpty?.Invoke();
                            }
                            SoundCentral.Instance.Invoke(HumanoidManager.PlayerTransform, SoundCentral.SoundTypes.Message);
                        }
                        // Debug.Log("soundRequest " + TextToDisplay);
                    }
                    catch (OperationCanceledException e)
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
                    //  Debug.Log("attempt cancel "+ source);
                    source?.Cancel();

                }
                async Task CoolDown()
                {
                    await Task.Delay(waitUntilNextDisplayMS);

                }
                void Fulfilled()
                {
                    // Debug.Log("fulfilled");
                    source = new CancellationTokenSource();
                    CancellationToken t = source.Token;
                    call?.Deafen(Fulfilled);
                    Run(t);




                }

                public Messages(string str, int waitUntilNextDisplayMS = 0, IMonoCall call = null)
                {
                    TextToDisplay = str;
                    this.waitUntilNextDisplayMS = waitUntilNextDisplayMS;
                    this.call = call;

                }
            }
        }
    }

    public class Scene2 : DefaultSpawnScene
    {
        public Scene2(GameManager manager) : base(manager)
        {
        
        }

        protected override void AdditionalCreation()
        {
            base.AdditionalCreation();
        }

        protected override void CleanUp()
        {
            base.CleanUp();
        }
    }

    public class DefaultSpawnScene : SceneCreation
    {   
      
        protected int enemiesLeft;

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
            if (SpawnData.exitPoint != null)
            {
                SpawnData.exitPoint.InExit += InEx;
                SpawnData.exitPoint.OutExit += NotInEx;
            }

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
            for (int i = 0; i < SpawnData.gunners.Count; i++)
            {
                EnemyGunner e = SpawnData.gunners[i];
                hu.CreateEnemy( e.transform.position,  e.gunID, e.ammoID, e.transform.eulerAngles.y, e.enemyPReFab);

            }


        }

        protected override void CleanUp()
        {   
            EnemyStuff.EnemyAI.EnemyKilled -= ReduceEnemy;
            if (SpawnData.exitPoint != null)
            {
                SpawnData.exitPoint.InExit -= InEx;
                SpawnData.exitPoint.InExit -= NotInEx;
            }
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