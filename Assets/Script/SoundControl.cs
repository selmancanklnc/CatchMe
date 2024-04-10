using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public static SoundControl Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Image musicButtonImage;
    public Image sfxButtonImage;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;


    public Button musicButton;
    public Button sfxButton;
    // Start is called before the first frame update
    void Start()
    {
        musicButton.onClick.AddListener(SoundControl.Instance.ToggleMusic);
        sfxButton.onClick.AddListener(SoundControl.Instance.ToggleSFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadAudioSettings();
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("MusicMuted", musicSource.mute ? 1 : 0);
        UpdateButtonImage(musicSource.mute, musicButtonImage, musicOnSprite, musicOffSprite);
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        PlayerPrefs.SetInt("SFXMuted", sfxSource.mute ? 1 : 0);
        UpdateButtonImage(sfxSource.mute, sfxButtonImage, sfxOnSprite, sfxOffSprite);
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

}
