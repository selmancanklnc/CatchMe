using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoMainMenuButon() 
    {
        OptionsSoundControl musicManager = OptionsSoundControl.GetInstance();
        if (musicManager != null)
        {
            Destroy(musicManager.gameObject);
            musicManager.StopMusic(); // Müziði durdur
        }

        SceneManager.LoadScene("MainMenu");
    }
   
}
