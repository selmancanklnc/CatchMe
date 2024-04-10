using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoggedWithGoogle : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject netControlPanel;
    public AudioListener audiolistener;
    public GameObject circleBar;
    public GameObject exitPanel;
    private FirebaseAuth auth;


    // Bu sahneye ilk defa gelinip gelinmediðini kontrol etmek için kullanýlan bir anahtar kelime
    private const string FirstTimeKey = "IsFirstTime";

    // Ýlk defa yüklendiðinde yönlendirmek istediðiniz sahnenin adý
    public string firstTimeSceneName;

    // Start is called before the first frame update
    void Start()
    {
        NetControl();

        auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {

            FirestoreExample.GetUser();

        }

    }

    public void NetControl()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            netControlPanel.SetActive(true);
            AudioListener.volume = 0;
        }
        else
        {
            netControlPanel.SetActive(false);
            AudioListener.volume = 1;

        }
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {

            if (exitPanel)
            {
                exitPanel.SetActive(true);
            }

        }

        NetControl();

    }


    public void MoveToScene()
    {
        if (PlayerPrefs.GetInt(FirstTimeKey, 0) == 0)
        {
            PlayerPrefs.SetInt(FirstTimeKey, 1); 
            SceneManager.LoadScene(4); 
        }
        else
        {
            SceneManager.LoadScene(2);

        }
    }

    public void onUserClickYesNo(int choice)
    {
        if (choice == 1)
        {
            circleBar.SetActive(true);
            exitPanel.SetActive(false);
            Application.Quit();
            circleBar.SetActive(false);
        }
        else
        {
            exitPanel.SetActive(false);
        }

    }
}
