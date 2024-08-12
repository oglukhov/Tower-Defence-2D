using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioClip battleMusic;
    public AudioClip mapMusic;

    private AudioSource audioSource;
    private static MusicManager instance;

    private Button playButton; // Change type to Button
    public bool musicState;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetInt("music", 1); // Default to music on
            PlayerPrefs.Save();
        }
        UpdateAudioState();

        // Ensure only one instance of MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMapMusic();
        FindPlayButton();
    }
    void Update(){
        if(PlayerPrefs.GetInt("music", 0) == 1){
            audioSource.volume = 1f;
        }
        if(PlayerPrefs.GetInt("music", 0) == 2){
            audioSource.volume = 0f;
        }
        musicState = ToggleButtonImage.musicOn;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Map")
        {
            PlayMapMusic();
            audioSource.volume = 1;
        }
        // Check and find the play button in the new scene
        FindPlayButton();
    }

    void FindPlayButton()
    {
        // Try to find the button named "StartBtn" in the scene
        GameObject playButtonObject = GameObject.Find("StartBtn");

        if (playButtonObject != null)
        {
            playButton = playButtonObject.GetComponent<Button>();
            if (playButton != null)
            {
                Debug.Log("Play button found and component assigned.");
                playButton.onClick.AddListener(OnPlayButtonClicked);
            }
            else
            {
                Debug.LogError("Button component not found on StartBtn.");
            }
        }
        else
        {
            Debug.LogError("StartBtn GameObject not found.");
        }
    }

    void PlayMapMusic()
    {
        if (audioSource.clip != mapMusic) 
        {
            Debug.Log("Playing map music.");
            audioSource.clip = mapMusic;
            audioSource.Play();
        }
    }

    void PlayBattleMusic()
    {
        if (audioSource.clip != battleMusic)
        {
            Debug.Log("Playing battle music.");
            audioSource.clip = battleMusic;
            audioSource.Play();
        }
    }

    void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked.");
        PlayBattleMusic();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (playButton != null)
        {
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
    }
    private void UpdateAudioState(){
        if(PlayerPrefs.GetInt("music", 0) == 1){
            ToggleButtonImage.musicOn = true;
        }
        if(PlayerPrefs.GetInt("music",0) == 2){
            ToggleButtonImage.musicOn = false;
        }
    }
}