using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundControl1 : MonoBehaviour
{

    public Button musicButton;
    public Button sfxButton;


    public Image musicButtonImage;
    public Image sfxButtonImage;
    public static SoundControl1 instance;
    private void Awake()
    {
        instance = this;
        musicButton.onClick.AddListener(tgm);
        sfxButton.onClick.AddListener(sfx);
    }
    public static SoundControl1 GetInstance()
    {
        return instance;
    }
    void tgm()
    {
        var optionsSoundControlInstance = OptionsSoundControl.GetInstance();
        optionsSoundControlInstance.ToggleMusic(musicButtonImage);
    }
    void sfx()
    {
        var optionsSoundControlInstance = OptionsSoundControl.GetInstance();
        optionsSoundControlInstance.ToggleSFX(sfxButtonImage);
    }

}
