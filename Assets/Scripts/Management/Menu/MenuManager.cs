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
   
       private void Awake()
       {
           _director = GameManager.Instance.director;
       }
   
       public void LevelsButton()
       {
           if (!_levelsActive)
           {
               _levelsActive = true;
               levelsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
               _director.Play(GameManager.Instance.playableAssets.Find(x => x.name == "OpenLevels"));
           }
           else
           {
               _levelsActive = false;
               levelsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Levels";
               _director.Play(GameManager.Instance.playableAssets.Find(x => x.name == "CloseLevels"));
           }
       }
       
       public void SetLevel(int level)
       {
           GameManager.Instance.Seed = level;
           GameManager.Instance.IsGameLoading = true;
           GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
       }
       
       public void EndlessLevel()
       {
           GameManager.Instance.IsGameLoading = true;
           GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
       }
   
       private void Update()
       {
           if(_introSkipped)
               return;
           if(_director.playableAsset.name != "Intro")
               return;
           if((Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame))
               SkipIntro();
       }
   
       private void SkipIntro()
       {
           _director.time = _director.duration - 1f;
           _director.Play();
           _introSkipped = true;
       }
       
       public void SetSaveName(TMP_InputField inputField)
       {
           SaveManager.Instance.FolderName = inputField.text;
       }
   } 
}

