using Management.SaveSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace Management.Menu
{
   public class MenuManager : MonoBehaviour
   {
       private PlayableDirector _director;
       
       [Header("New Game Buttons")]
       [SerializeField] private Button levelsButton;
       private bool _levelsActive;
       
       [SerializeField] private bool _introSkipped;
   
       /// <summary>
       /// Gets a reference to the director component from the GameManager singleton instance.
       /// </summary>
       private void Awake()
       {
           // Assign the director component to a private field
           _director = Management.GameManager.Instance.director;
       }
   
       /// <summary>
       /// Toggles the levels menu and plays the corresponding animation using the director component.
       /// </summary>
       public void LevelsButton()
       {
           // If the levels menu is not active, activate it and change the button text to "Back"
           if (!_levelsActive)
           {
               _levelsActive = true;
               levelsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
               // Play the "OpenLevels" animation from the playable assets list
               _director.Play(Management.GameManager.Instance.playableAssets.Find(x => x.name == "OpenLevels"));
           }
           // Else, deactivate the levels menu and change the button text to "Levels"
           else
           {
               _levelsActive = false;
               levelsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Levels";
               // Play the "CloseLevels" animation from the playable assets list
               _director.Play(Management.GameManager.Instance.playableAssets.Find(x => x.name == "CloseLevels"));
           }
       }
       
       /// <summary>
       /// Sets the seed value for the game manager and loads the game scene asynchronously.
       /// </summary>
       /// <param name="level">int - The seed value for the game manager.</param>
       public void SetLevel(int level)
       {
           // Set the seed value for the game manager
           Management.GameManager.Instance.Seed = level;
           // Set the game loading flag to true
           Management.GameManager.Instance.IsGameLoading = true;
           // Add the game scene loading operation to the loading operations list
           Management.GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
       }
       
       /// <summary>
       /// Loads the game scene asynchronously for the endless mode.
       /// </summary>
       public void EndlessLevel()
       {
           // Set the game loading flag to true
           Management.GameManager.Instance.IsGameLoading = true;
           // Add the game scene loading operation to the loading operations list
           Management.GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
       }
   
       /// <summary>
       /// Checks for user input to skip the intro animation if it is playing.
       /// </summary>
       private void Update()
       {
           // If the intro has already been skipped, do nothing
           if(_introSkipped)
               return;
           // If the current animation is not the intro, do nothing
           if(_director.playableAsset.name != "Intro")
               return;
           // If any key or the left mouse button was pressed this frame and the director state is playing, call the SkipIntro method
           if((Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame) && _director.state == PlayState.Playing)
               SkipIntro();
       }
   
       /// <summary>
       /// Skips the intro animation by setting the director time to near the end and playing it.
       /// </summary>
       private void SkipIntro()
       {
           // Set the director time to one second before the end of the animation
           _director.time = _director.duration - 1f;
           // Play the animation from that point
           _director.Play();
           // Set the intro skipped flag to true
           _introSkipped = true;
       }
       
       /// <summary>
       /// Sets the save folder name for the SaveManager based on the input field text.
       /// </summary>
       /// <param name="inputField">TMP_InputField - The input field that contains the folder name.</param>
       public void SetSaveName(TMP_InputField inputField)
       {
           // Set the folder name for the SaveManager to the input field text
           SaveManager.Instance.FolderName = inputField.text;
       }
   } 
}

