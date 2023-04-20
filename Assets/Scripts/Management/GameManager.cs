using System.Collections.Generic;
using System.Linq;
using Management.SaveSystem;
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

        // Loading Screen
        private bool IsGameLoading { get; set; }
        private List<AsyncOperation> LoadingOperations { get; set; }
        
        // Timeline
        public PlayableDirector director;
        public List<PlayableAsset> playableAssets;

        /// <summary>
        /// This method is called when the script instance is being loaded. It checks if there is another instance of this script in the scene and destroys it if yes. Otherwise, it assigns this script to the singleton instance field and makes it persistent across scenes. It also gets the PlayableDirector, SaveManager, Volume, and VolumeProfile components attached to the game object and assigns them to private fields. It also initializes the loading operations list, the game loading flag, and the save manager.
        /// </summary>
        /// <remarks>
        /// It requires that the game object has a GameManager script, a PlayableDirector component, a SaveManager script, and a Volume component attached to it. It also requires that the volume component has a volume profile assigned to it.
        /// </remarks>
        private void Awake()
        {
            // Check if there is another instance of this script in the scene
            if (Instance != null && Instance != this)
                // If yes, destroy this game object
                Destroy(gameObject);
            else
            {
                // If not, assign this script to the instance field
                Instance = this;
                // Make this game object persistent across scenes
                DontDestroyOnLoad(gameObject);
            }
            // Get the PlayableDirector component and assign it to the director field
            director = GetComponent<PlayableDirector>();
            
            // Create a new list of AsyncOperation objects and assign it to the loading operations field
            LoadingOperations = new List<AsyncOperation>();
            // Set the game loading flag to false
            IsGameLoading = false;
            // Get the SaveManager script and assign it to the save manager field
            saveManager = GetComponent<SaveManager>();
            // Get the Volume component and assign it to the global volume field
            globalVolume = GetComponent<Volume>();
            // Get the volume profile from the volume component and assign it to the profile field
            profile = globalVolume.profile;
        }

        /// <summary>
        /// This method is called once per frame. It calls the LoadingScreen method to show or hide the loading screen based on the game loading flag.
        /// </summary>
        private void Update()
            // Use the expression-bodied method syntax to call the LoadingScreen method
            => LoadingScreen();

        /// <summary>
        /// This method loads a scene asynchronously by its name and adds it to the loading operations list. It also sets the game loading flag to true.
        /// </summary>
        /// <param name="sceneName">string - The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            // Set the game loading flag to true
            IsGameLoading = true;
            // Load the scene asynchronously by its name and add it to the loading operations list
            LoadingOperations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single));
        }

        /// <summary>
        /// This method shows or hides the loading screen based on the game loading flag and the completion of the loading operations.
        /// </summary>
        private void LoadingScreen()
        {
            // Check if the game is not loading
            if (!IsGameLoading) 
                // If yes, return from the method
                return;
            // If not, play the Start Loading timeline asset using the director
            director.Play(playableAssets.Find(x => x.name == "Start Loading"));
            // Check if all the loading operations are done
            var isDone = LoadingOperations.All(operation => operation.isDone);
            // If not, return from the method
            if (!isDone) 
                return;
            // If yes, set the game loading flag to false
            IsGameLoading = false;
            // Clear the loading operations list
            LoadingOperations.Clear();
            // Play the Stop Loading timeline asset using the director
            director.Play(playableAssets.Find(x => x.name == "Stop Loading"));
        }
    }
}

