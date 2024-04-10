using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{

    public Button[] buttons;

    public Sprite defaultSprite;
    public Sprite selectedSprite;
    private int level;


    public GameObject Home;
    public GameObject detailPanel;
    public GameObject purchasePanel;
    public GameObject settingsPanel;
    public GameObject leaderboardPanel;
    public GameObject buyPanel;
    public GameObject lvlText;
    public GameObject finalchapter;






    List<Button> newButtons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        //FirestoreExample.GetUser2();
        ////Screen.orientation = ScreenOrientation.LandscapeLeft;
        ////PlayerPrefs.SetInt("Level", 41);
        //Config.UserLevel = PlayerPrefs.GetInt("Level", 41);
        //Config.CurrentLevel = PlayerPrefs.GetInt("Level", 41);


        level = Config.UserLevel;
        //Config.UserLevel = level;

        //storedIndex = PlayerPrefs.GetInt("buttonIndex", -1);
        InitializeButtons();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (finalchapter.activeSelf)
        {
            lvlText.SetActive(false);
        }
        else 
        { 
            lvlText.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Home.activeSelf)

            {
                if (detailPanel.activeSelf || purchasePanel.activeSelf || settingsPanel.activeSelf || leaderboardPanel.activeSelf || buyPanel.activeSelf)
                {

                    detailPanel.SetActive(false);
                    purchasePanel.SetActive(false);
                    settingsPanel.SetActive(false);
                    leaderboardPanel.SetActive(false);
                    buyPanel.SetActive(false);

                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            }

        }
    }



    private void InitializeButtons()
    {
        var chapters = GameObject.FindGameObjectsWithTag("chapter").OrderBy(a => Convert.ToInt32(a.name.Replace("Chapter ", ""))).ToList();


        foreach (var chapter in chapters)
        {
            if (chapter.name != "Chapter 13")
            {

                foreach (var item in buttons)
                {
                    var button = Instantiate(item, item.transform.position, item.transform.rotation, chapter.transform);
                    newButtons.Add(button); 
                }
            }

        }
        foreach (var chapter in chapters)
        {
            if (chapter != chapters.FirstOrDefault(a => a.name == $"Chapter {Config.UserChapter}"))
            {
                chapter.SetActive(false);
            }
        }

        for (int i = 0; i < newButtons.Count; i++)
        {
            if (level >= i)
            {
                var rawImage = newButtons[i].GetComponentInChildren<RawImage>();
                rawImage.gameObject.SetActive(false);

            }
            else
            {
                newButtons[i].interactable = false;
            }
            var selectedIndex = i;
            newButtons[i].GetComponent<Image>().sprite = (selectedIndex == level) ? selectedSprite : defaultSprite;
            newButtons[i].onClick.AddListener(() => OnButtonClick(selectedIndex));
        }
    }

    private void OnButtonClick(int buttonIndex)
    {

        Config.CurrentLevel = buttonIndex;
        //PlayerPrefs.Save();

        for (int i = 0; i < newButtons.Count; i++)
        {

            if (i == buttonIndex)
            {
                newButtons[i].GetComponent<Image>().sprite = selectedSprite;

            }
            else
            {
                newButtons[i].GetComponent<Image>().sprite = defaultSprite;
            }
        }

    }


}
