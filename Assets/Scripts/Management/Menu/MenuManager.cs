using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Management.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private GameObject _currentMenu;
        [Header("Menu UIs")]
            [SerializeField] private GameObject mainMenu;
            [SerializeField] private GameObject creditsMenu;
            [SerializeField] private GameObject settingsMenu;
        
        [Header("Settings UIs")]
            [SerializeField] private GameObject audioMenu;
            [SerializeField] private GameObject graphicsMenu;
            [SerializeField] private GameObject gameplayMenu;
            
        [Header("Cover")]
            [SerializeField] private GameObject cover;

        private void Awake()
        {
            _currentMenu = mainMenu;
        }
        
        /// <summary>
        /// This method is called before the first frame update. It deactivates all the menu game objects.
        /// This way, the Settings Save/Load system can work properly.
        /// </summary>
        private void Start()
        {
            StartCoroutine(WaitForSettings());
        }
        
        private IEnumerator WaitForSettings()
        {
            var audioManager = audioMenu.GetComponent<AudioManager>();
            var graphicsManager = graphicsMenu.GetComponent<VideoManager>();
            
            // Wait until the settings are loaded
            yield return new WaitUntil(() => audioManager.settingsLoaded && graphicsManager.settingsLoaded);
            
            cover.SetActive(false);
            // Deactivate the main menu game object
            mainMenu.SetActive(false);
            // Deactivate the credits menu game object
            creditsMenu.SetActive(false);
            // Deactivate the settings menu game object
            settingsMenu.SetActive(false);
            // Deactivate the audio menu game object
            audioMenu.SetActive(false);
            // Deactivate the graphics menu game object
            graphicsMenu.SetActive(false);
            // Deactivate the gameplay menu game object
            gameplayMenu.SetActive(false);
        }
        

        /// <summary>
        /// This method is called once per frame. It checks if the escape key was pressed and calls the Return method if it was.
        /// </summary>
        private void Update()
        {
            // If the escape key was pressed this frame
            if(Keyboard.current.escapeKey.wasPressedThisFrame)
                // Call the Return method
                Return();
        }

        /// <summary>
        /// This method is called when the user wants to return to the previous menu or close the current menu. It toggles the active state of the current menu and switches to the appropriate menu. It also locks or unlocks the cursor depending on the menu state.
        /// </summary>
        public void Return()
        {
            // If the current menu is the main menu
            if (_currentMenu == mainMenu)
            {
                // Toggle the active state of the current menu
                _currentMenu.SetActive(!_currentMenu.activeSelf);
                // Lock or unlock the cursor depending on the active state of the current menu
                CursorLock(!_currentMenu.activeSelf);
            }
            // Else if the current menu is the settings menu
            else if (_currentMenu == settingsMenu)
            {
                // Deactivate the current menu
                _currentMenu.SetActive(false);
                // Set the current menu to the main menu
                _currentMenu = mainMenu;
                // Activate the current menu
                _currentMenu.SetActive(true);
            }
            // Else if the current menu is one of the submenus of the settings menu (audio, graphics, or gameplay)
            else if(_currentMenu == audioMenu || _currentMenu == graphicsMenu || _currentMenu == gameplayMenu)
            {
                // Deactivate the current menu
                _currentMenu.SetActive(false);
                // Set the current menu to the settings menu
                _currentMenu = settingsMenu;
                // Activate the current menu
                _currentMenu.SetActive(true);
            }
            // Else if the current menu is the credits menu
            else if (_currentMenu == creditsMenu)
            {
                // Deactivate the credits menu
                creditsMenu.SetActive(false);
                // Set the current menu to the main menu
                _currentMenu = mainMenu;
                // Activate the current menu
                _currentMenu.SetActive(true);
            }
        }
        
        /// <summary>
        /// This method is called when the user wants to go to a specific menu. It takes a string parameter that represents the target menu. It deactivates the current menu and activates the target menu.
        /// </summary>
        /// <param name="target">
        /// string - the name of the target menu. It can be one of the following values: "Settings", "Audio", "Graphics", "Gameplay", or "Credits".
        /// </param>
        public void GoTo(string target)
        {
            // Use a switch statement to handle different target values
            switch (target)
            {
                // If the target is "Settings"
                case "Settings":
                    // Deactivate the current menu
                    _currentMenu.SetActive(false);
                    // Set the current menu to the settings menu
                    _currentMenu = settingsMenu;
                    // Activate the current menu
                    _currentMenu.SetActive(true);
                    break;
                // If the target is "Audio"
                case "Audio":
                    // Deactivate the current menu
                    _currentMenu.SetActive(false);
                    // Set the current menu to the audio menu
                    _currentMenu = audioMenu;
                    // Activate the current menu
                    _currentMenu.SetActive(true);
                    break;
                // If the target is "Graphics"
                case "Graphics":
                    // Deactivate the current menu
                    _currentMenu.SetActive(false);
                    // Set the current menu to the graphics menu
                    _currentMenu = graphicsMenu;
                    // Activate the current menu
                    _currentMenu.SetActive(true);
                    break;
                // If the target is "Gameplay"
                case "Gameplay":
                    // Deactivate the current menu
                    _currentMenu.SetActive(false);
                    // Set the current menu to the gameplay menu
                    _currentMenu = gameplayMenu;
                    // Activate the current menu
                    _currentMenu.SetActive(true);
                    break;
                // If the target is "Credits"
                case "Credits":
                    // Deactivate the current menu
                    _currentMenu.SetActive(false);
                    // Set the current menu to the credits menu
                    _currentMenu = creditsMenu;
                    // Activate the current menu
                    _currentMenu.SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// This method is a static method that locks or unlocks the cursor based on a boolean parameter. It also sets the cursor visibility and the time scale accordingly.
        /// </summary>
        /// <param name="lockCursor">
        /// bool - a flag that indicates whether to lock or unlock the cursor. If true, the cursor is locked and hidden and the time scale is 1. If false, the cursor is unlocked and visible and the time scale is 0.
        /// </param>
        private static void CursorLock(bool lockCursor)
        {
            // Set the cursor lock state based on the parameter
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            // Set the cursor visibility to the opposite of the parameter
            Cursor.visible = !lockCursor;
            // Set the time scale to 1 if the parameter is true, or 0 if the parameter is false
            Time.timeScale = lockCursor ? 1 : 0;
        }

        /// <summary>
        /// This method is called when the user wants to return to the hub scene. It calls the LoadScene method of the GameManager instance with the scene name "Hub".
        /// </summary>
        public void ReturnToHub()
        {
            Return();
            // Call the LoadScene method of the GameManager instance with the scene name "Hub"
            Management.GameManager.Instance.LoadScene("Hub");
        }

        /// <summary>
        /// This method is called when the application quits. It unlocks the cursor from the center of the screen.
        /// </summary>
        private void OnApplicationQuit()
        {
            // Call the CursorLock method with false as the argument to unlock the cursor
            CursorLock(false);
        }

        /// <summary>
        /// This method is called when the user wants to exit the game. It checks if the game is running in the editor or in a build and calls the appropriate method to stop or quit the game.
        /// </summary>
        public void ExitGame()
        {
            // Use preprocessor directives to check if the game is running in the editor or in a build
            #if UNITY_EDITOR
                // If the game is running in the editor, set the isPlaying property of the EditorApplication class to false
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                // If the game is running in a build, call the Quit method of the Application class
                Application.Quit();
            #endif
        }
    }
}