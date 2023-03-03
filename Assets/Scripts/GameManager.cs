using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    enum CurrentScene
    {
        MainMenu,
        Game,
    }
    private PlayableDirector _director;

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
    }




}
