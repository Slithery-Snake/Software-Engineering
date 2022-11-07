using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


[System.Serializable]
public static class Constants
{
    
    public static int playerMask = 6;
    public static int enemyMask = 7;
    public static int playerCamIgnoremask = 8;
}
public class GameStateEvent : UnityEvent<GameManager.GameState, GameManager.GameState>
{

}

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu, InGame, Paused
    }
    int sceneIndex;
    AsyncOperation loadingOperation;
    AsyncOperation unloadingOperation;
    List<AsyncOperation> operations;

    public GameStateEvent GameStateChanged;
    GameState gameState = GameState.InGame;

    [SerializeField] BulletSpawn bulletSpawn;
    [SerializeField] ItemManager itemManager;
    [SerializeField] HumanoidManager humanoidManager;
   
    public GameState GetGameState
    {
        get { return gameState; }
        private set { gameState = value; }
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
        BulletSpawn bSpawn;
        ItemManager itemManager;

        public Scene1(int index, GameManager manager) : base(index, manager)
        {
            Create();
        }

        public override void Create()
        {   
            bSpawn = BulletSpawn.Create(manager, manager.bulletSpawn);
            itemManager = ItemManager.CreateItemManager(manager.itemManager, bSpawn);
            HumanoidManager.Create(manager.humanoidManager, itemManager);
        }
    }

    void Start()
    {
        
        DontDestroyOnLoad(gameObject);  // the game managers gameobject does not destroy when loading other scenes
        operations = new List<AsyncOperation>();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneIndex));
        Scene1 scene1 = new Scene1(1, this);
    }
    private void Update()
    {

    }

    public void SetActive(int sceneIndex)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }
    public void LoadScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        if (loadingOperation != null) { return; }
        loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        operations.Add(loadingOperation);
        loadingOperation.completed += LoadOperationDone;

    }

    public void UnloadScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        if (unloadingOperation != null) { return; }
        unloadingOperation = SceneManager.UnloadSceneAsync(sceneIndex);
        operations.Add(unloadingOperation);
        unloadingOperation.completed += UnloadOperationDone;

    }
    void UnloadOperationDone(AsyncOperation ao)
    {

    }
    void LoadOperationDone(AsyncOperation ao)
    {
        if (operations.Contains(loadingOperation))
        {
            operations.Remove(loadingOperation);
            if (operations.Count == 0)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneIndex));
                UpdateState(GameState.InGame);
            }



        }

    }


    void UpdateState(GameState newState)
    {
        GameState previousGameState = gameState;
        gameState = newState;
        switch (gameState)
        {
            case GameState.Menu:
                Pause(false);
                break;
            case GameState.InGame:
                Pause(false);
                break;
            case GameState.Paused:
                Pause(true);
                break;
            default:
                break;
        }

        GameStateChanged.Invoke(gameState, previousGameState); //game state changed event
    }
    public void StartGame()
    {
        LoadScene(0);
    }

    public void TogglePause()
    {
        UpdateState(gameState == GameState.InGame ? GameState.Paused : GameState.InGame);

    }
    void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
    public void Quit()
    {
        Debug.Log("to the menu");
    }
    public void Restart()
    {
        LoadScene(sceneIndex);
    }



}