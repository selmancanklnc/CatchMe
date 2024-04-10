using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsSoundControl : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;


 

    Image musicButtonImage;
    Image sfxButtonImage;

    public static OptionsSoundControl instance;


    private string currentSceneName; // Þu anki sahnenin adýný tutmak için kullanacaðýz

    public AudioClip yeniAudioClip;
    public AudioClip eskiAudioClip;

    void Start()
    {

        if (Config.CurrentChapter > 3)
        {
            musicSource.clip = yeniAudioClip;
        }
        else
        {
            musicSource.clip = eskiAudioClip;
        }
        
        LoadAudioSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSource == null)
        {
            musicSource.Play();

        }
        else if (!musicSource.isPlaying)
        {
            musicSource.Play();

        }
    }
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        var buttonInstanse = SoundControl1.GetInstance();
        //musicButton = buttonInstanse.musicButton;
        //sfxButton = buttonInstanse.sfxButton;
        musicButtonImage = buttonInstanse.musicButtonImage;
        sfxButtonImage = buttonInstanse.sfxButtonImage;
        //musicButton.onClick.AddListener(ToggleMusic);
        //sfxButton.onClick.AddListener(ToggleSFX);
        SceneManager.sceneLoaded += OnSceneChanged;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (instance != this)
        {
            return;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
    public void ToggleMusic(Image buttonImage)
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("MusicMuted", musicSource.mute ? 1 : 0);
        UpdateButtonImage(musicSource.mute, buttonImage, musicOnSprite, musicOffSprite);
    }

    public void ToggleSFX(Image buttonImage)
    {
        sfxSource.mute = !sfxSource.mute;
        PlayerPrefs.SetInt("SFXMuted", sfxSource.mute ? 1 : 0);
        UpdateButtonImage(sfxSource.mute, buttonImage, sfxOnSprite, sfxOffSprite);
    }

    private void LoadAudioSettings()
    {
        musicSource.mute = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxSource.mute = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        UpdateButtonImage(musicSource.mute, musicButtonImage, musicOnSprite, musicOffSprite);
        UpdateButtonImage(sfxSource.mute, sfxButtonImage, sfxOnSprite, sfxOffSprite);
    }

    private void UpdateButtonImage(bool isMuted, Image buttonImage, Sprite onSprite, Sprite offSprite)
    {
        buttonImage.sprite = isMuted ? offSprite : onSprite;
    }
    private void OnSceneChanged(Scene newScene, LoadSceneMode mode)
    {
        // Sahne deðiþtiðinde, yeni sahnenin adýný al
        currentSceneName = newScene.name;
        // Sahne adýný kontrol ederek müziði aç/kapat/tekrar baþlat
        if (currentSceneName != "CatchMeGame")
        {
            //gamemusicSource?.Stop(); // Diðer sahnelerde müziði kapat
            Destroy(instance?.gameObject);

        }


    }
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public static OptionsSoundControl GetInstance()
    {
        return instance;
    }

}
