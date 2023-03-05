using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private PlayableDirector _director = GameManager.Instance.director;
    
    [Header("New Game Buttons")]
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button[] levels;
    private bool _levelsActive;

    public void LevelsButton()
    {
        if (!_levelsActive)
        {
            _levelsActive = true;
            levelsButton.GetComponentInChildren<Text>().text = "Back";
            _director.Play(GameManager.Instance.playableAssets.Find(x => x.name == "OpenLevels"));
        }
        else
        {
            _levelsActive = false;
            levelsButton.GetComponentInChildren<Text>().text = "Levels";
            _director.Play(GameManager.Instance.playableAssets.Find(x => x.name == "CloseLevels"));
        }
    }
    
    public void SetLevel(int level)
    {
        GameManager.Instance.Seed = level;
        GameManager.Instance.RandomSeed = false;
        GameManager.Instance.IsGameLoading = true;
        GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
    }
    
    public void EndlessLevel()
    {
        GameManager.Instance.RandomSeed = true;
        GameManager.Instance.IsGameLoading = true;
        GameManager.Instance.LoadingOperations.Add(SceneManager.LoadSceneAsync("Game"));
    }


}
