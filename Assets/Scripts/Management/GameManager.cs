using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // Loading Screen
    public bool IsGameLoading { get; set; }
    public List<AsyncOperation> LoadingOperations { get; private set; }
    
    // Current Scene
    public enum CurrentScene
    {
        MainMenu,
        Game,
        LoadingScreen
    }
    [Header("Loading Screen")]
    public CurrentScene currentScene;

    // Timeline
    public PlayableDirector director;
    public List<PlayableAsset> playableAssets;

    [Header("Maze Settings")]
    [SerializeField] private int seed;
    public int Seed { get => seed; set => seed = value;}
    [SerializeField] private bool randomSeed;
    public bool RandomSeed { get => randomSeed; set => randomSeed = value;}


    /// <summary>
    /// If there is no instance of GameManager, set this instance to be the instance.<br/>
    /// Also, don't destroy this instance when loading a new scene.<br/>
    /// If there is an instance of GameManager, destroy this instance.<br/>
    /// set the variables of the GameManager.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        director = GetComponent<PlayableDirector>();
        
        LoadingOperations = new List<AsyncOperation>();
        IsGameLoading = false;
    }

    private void Update()
    {
        LoadingScreen();
    }
    
    /// <summary>
    /// Load the game scene.
    /// </summary>
    public void LoadGame()
    {
        IsGameLoading = true;
        LoadingOperations.Add(SceneManager.LoadSceneAsync((int)CurrentScene.Game, LoadSceneMode.Additive));
    }
    
    /// <summary>
    /// Load the main menu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        IsGameLoading = true;
        LoadingOperations.Add(SceneManager.LoadSceneAsync((int)CurrentScene.MainMenu, LoadSceneMode.Additive));
    }
    
    /// <summary>
    /// It will play the timeline of the loading screen and will check if the game is loading.<br/>
    /// While the game is loading, it will play the timeline of the loading screen.<br/>
    /// It uses the LoadingOperations list to check if the game is loading.<br/>
    /// When the game is done loading, it will play the timeline of the end of the loading screen.<br/>
    /// </summary>
    private void LoadingScreen()
    {
        if (IsGameLoading)
        {
            currentScene = CurrentScene.LoadingScreen;
            director.Play(playableAssets.Find(x => x.name == "Start Loading"));
            var isDone = true;
            foreach (var operation in LoadingOperations)
            {
                if (!operation.isDone)
                {
                    isDone = false;
                    break;
                }
            }
            if (isDone)
            {
                IsGameLoading = false;
                LoadingOperations.Clear();
                director.Play(playableAssets.Find(x => x.name == "Stop Loading"));
                currentScene = GetCurrentSceneName() == "Game" ? CurrentScene.Game : CurrentScene.MainMenu;
            }
        }
    }
    
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    /// <summary>
    /// Locks and hides the cursor if the parameter is true.
    /// Unlocks and shows the cursor if the parameter is false.
    /// </summary>
    /// <param name="lockCursor">bool - state of the cursor</param>
    private static void CursorLock(bool lockCursor)
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
    }

    private void OnApplicationQuit()
    {
        CursorLock(false);
    }
}
