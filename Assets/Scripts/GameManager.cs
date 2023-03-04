using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // Loading Screen
    public bool IsGameLoading { get; private set; }
    public List<AsyncOperation> LoadingOperations { get; private set; }

    // Current Scene
    public enum CurrentScene
    {
        MainMenu,
        Game,
        LoadingScreen
    }
    public CurrentScene currentScene;
    
    // Timeline
    private PlayableDirector _director;
    [SerializeField] private List<PlayableAsset> playableAssets;

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
        _director = GetComponent<PlayableDirector>();
        
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
            _director.Play(playableAssets.Find(x => x.name == "Start Loading"));
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
                _director.Play(playableAssets.Find(x => x.name == "End Loading"));
                currentScene = GetCurrentSceneName() == "Game" ? CurrentScene.Game : CurrentScene.MainMenu;
            }
        }
    }
    
    public static string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }





}
