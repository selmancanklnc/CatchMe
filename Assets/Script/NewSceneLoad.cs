using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 0);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewScene() { SceneManager.LoadScene("CatchMeGame"); }


}
