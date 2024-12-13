using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] private string _currentSceneName;
    private static AudioSource _audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate AudioManager
            return;
        }
        else
        {
            instance = this;  // Set the instance to this AudioManager
        }

        // Ensure the AudioManager persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SetAudioClip();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _currentSceneName = scene.name;
        SetAudioClip();
    }

    private void SetAudioClip()
    {
        switch (_currentSceneName)
        {
            case "MainSCN":
                ChangeAudioClip(GameManager.instance.GetAudioList().MainMenuBGM);
                break;
                
            case "GameSCN":
                ChangeAudioClip(GameManager.instance.GetAudioList().ForestBGM);
                break;
                
            case "GameOver":

                break;
                
            default:
                Debug.Log("Unknown scene!");
                break;
        }
    }

    private void ChangeAudioClip(AudioClip clip)
    {
        if (_audioSource == null)
        {
            return;
        }
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
