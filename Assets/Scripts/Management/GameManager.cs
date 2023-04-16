using System.Collections.Generic;
using System.Linq;
using Management.SaveSystem;
using MazeSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public SaveManager saveManager;
        public Volume globalVolume;
        public VolumeProfile profile;
        [HideInInspector] public MazeGenerator mazeGenerator;

        // Loading Screen
        public bool IsGameLoading { get; set; }
        public List<AsyncOperation> LoadingOperations { get; private set; }
        
        // Save Data
        public bool IsGameLoaded { get; set; }

        // Timeline
        public PlayableDirector director;
        public List<PlayableAsset> playableAssets;
        
        // Enemy
        public List<GameObject> enemiesAlive = new();
        public Vector3[] enemyPositions;
        public Quaternion[] enemyRotations;
        public int[] enemyHealths;

        [Header("Maze Settings")]
        [SerializeField] private int seed;
        public int Seed { get => seed; set => seed = value;}
        [SerializeField] private Vector2Int mazeSize;
        public Vector2Int MazeSize { get => mazeSize; set => mazeSize = value; }
    
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
            saveManager = GetComponent<SaveManager>();
            globalVolume = GetComponent<Volume>();
            profile = globalVolume.profile;
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
            LoadingOperations.Add(SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single));
        }
        
        /// <summary>
        /// Load the main menu scene.
        /// </summary>
        public void LoadMainMenu()
        {
            IsGameLoading = true;
            LoadingOperations.Add(SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single));
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
        
        public void SaveData()
        {
            SaveMazeData();
            
            SaveManager.Instance.Save();
        }
        
        public void LoadData()
        {
            LoadMazeData();
        }

        private void SaveMazeData()
        {
            saveManager.dataClass.mazeSeed = seed;
            saveManager.dataClass.mazeSize = mazeSize;

            saveManager.dataClass.enemyPositions = enemiesAlive.Select(selectedEnemy => selectedEnemy.transform.position).ToArray();
            saveManager.dataClass.enemyRotations = enemiesAlive.Select(selectedEnemy => selectedEnemy.transform.rotation).ToArray();
            saveManager.dataClass.enemyHealths = enemiesAlive.Select(selectedEnemy => selectedEnemy.GetComponent<NPC.Stats>().health).ToArray();
        }
        
        private void LoadEnemies()
        {
            enemyPositions = SaveManager.Instance.dataClass.enemyPositions;
            enemyRotations = SaveManager.Instance.dataClass.enemyRotations;
            enemyHealths = SaveManager.Instance.dataClass.enemyHealths;
        }
        
        private void LoadMazeData()
        {
            seed = saveManager.dataClass.mazeSeed;
            mazeSize = saveManager.dataClass.mazeSize;
        }
        
        public void OnGameLoaded()
        {
            for (var i = 0; i < enemyHealths.Length; i++)
                enemiesAlive[i].GetComponent<NPC.Stats>().health = enemyHealths[i];
        }
        
        public void RegisterEnemy(GameObject enemy)
        {
            enemiesAlive.Add(enemy);
        }

        public void UnregisterEnemy(GameObject enemy)
        {
            enemiesAlive.Remove(enemy);
        }

        public void OnMazeExit()
        {
            
        }
        
        public void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}

