using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Kare : MonoBehaviour
{

    public GameObject floorButton;
    public GameObject floorClose;
    public GameObject player;
    public GameObject player2;
    public GameObject skipControl;
    public GameObject pauseButton;
    public GameObject pausePanel;
    public GameObject gameoverPanel;
    public GameObject gameWinPanel;
    public GameObject animatedCamera; // Animasyonu oynayan GameObject
    public GameObject animatedCamera2; // Animasyonu oynayan GameObject
    private Animator animator;
    public Material stoneMaterial;
    public Material metalMaterial;
    public static float time = 0;
    bool startTimer = false;
    //bool isWin = false;
    public TMP_Text timeText;
    public GameObject timeBox;
    public Cubemap newskyboxMaterial;
    public Cubemap mainskyboxMaterial;

    public Material mainCharacterMaterial;
    public Material hardlvlCharacterMaterial;
    public GameObject Character1;
    public GameObject Character2;

    public GameObject gameStartPanel;
    public RawImage avatarRawImage;
    public TMP_Text gamelvltext;
    public GameObject trainerParent;
    public GameObject[] trainerList;





    void Start()
    {
        PlayerPrefs.SetInt("ClickCount", 0);
        Config.closedObjects = new List<GameObject>();
        if (Config.CurrentChapter < 7)
        {
            animator = animatedCamera.GetComponent<Animator>();

        }
        else
        {
            animatedCamera.SetActive(false);
            animatedCamera2.SetActive(true);
            animator = animatedCamera2.GetComponent<Animator>();
        }

        LevelTrainer();
        LoadAvatarImage();
        Renderer rendererCharacter1 = Character1.GetComponent<Renderer>();
        Renderer rendererCharacter2 = Character2.GetComponent<Renderer>();


        var level = Config.CurrentLevel;
        if (Config.CurrentChapterDifficult > 4)
        {
            time = 60;
        }
        if (Config.CurrentChapterDifficult > 3)
        {
            Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
            skyboxMaterial.SetTexture("_Tex", newskyboxMaterial);
            RenderSettings.skybox = skyboxMaterial;
            rendererCharacter1.material = hardlvlCharacterMaterial;
            rendererCharacter2.material = hardlvlCharacterMaterial;


        }
        else
        {
            Material skyboxMaterial = new Material(Shader.Find("Skybox/Cubemap"));
            skyboxMaterial.SetTexture("_Tex", mainskyboxMaterial);
            RenderSettings.skybox = skyboxMaterial;
            rendererCharacter1.material = mainCharacterMaterial;
            rendererCharacter2.material = mainCharacterMaterial;
        }



        //BU KOD SÝLÝNECEK!!! *HATIRLATMA*
        //if (level > 2)
        //{
        //    level = 0;
        //    PlayerPrefs.SetInt("Level", level);
        //}

        GoToEndOfAnimation();
        int i = 1;
        floorButton.name = $"floor{i}";
        var positon = 0;
        var firstPosition = floorButton.transform.position;
        var firstPositionX = firstPosition.x;
        var firstPositionY = firstPosition.y;
        var firstPositionZ = firstPosition.z;
        var rectTransform = floorButton.GetComponent<Transform>();
        var floors = new List<GameObject>();
        var width = rectTransform.localScale.x * 1.2f;
        var playerPosition = (int)Config.ColAndRowCount / 2;
        for (int row = 0; row < Config.ColAndRowCount; row++)
        {
            for (int col = 0; col < Config.ColAndRowCount; col++)
            {

                if (col == 0 && row == 0)
                {
                    continue;
                }
                float positonX = firstPositionX + width * row;

                var positonZ = firstPositionZ - width * col;
                var button = Instantiate(floorButton, new Vector3(positonX, firstPositionY, positonZ), rectTransform.rotation);
                i++;
                button.name = $"floor{i}";
                if (col == playerPosition && row == playerPosition)
                {
                    player.transform.position = button.transform.position;
                    //player.transform.position = new Vector3(button.transform.position.x, playerY, button.transform.position.z); ;
                    positon = i;
                }
                floors.Add(button);
            }
        }
        if (Config.CurrentChapterDifficult > 5)
        {
            PlayerPrefs.SetInt("ClickCount", 0);
        }
        if (Config.CurrentChapterDifficult < 6)
        {
            floors.ShuffleMe();
            var newFloors = floors.Where(a => a.name != $"floor{positon}").Take(10 - level % 10).ToList();
            foreach (var item in newFloors)
            {
                // item.GetComponent<MeshRenderer>().material = floorCloseMaterial;
                Instantiate(floorClose, new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z), rectTransform.rotation);
                item.SetActive(false);
            }
        }

        if (Config.CurrentChapterDifficult > 1)
        {
            floors.ShuffleMe();
            var stoneFloors = floors.Where(a => a.activeSelf).Take(level % 10 + 1).ToList();
            foreach (var item in stoneFloors)
            {
                item.GetComponent<Renderer>().material = stoneMaterial;
            }
        }
        if (Config.CurrentChapterDifficult > 2)
        {
            floors.ShuffleMe();
            var metalFloors = floors.Where(a => a.activeSelf && a.GetComponent<Renderer>().material != stoneMaterial).Take(level % 10 + 1).ToList();
            foreach (var item in metalFloors)
            {

                item.GetComponent<Renderer>().material = metalMaterial;
            }
        }

        gamelvltext.text = $"Chapter ({Config.CurrentChapter})\nLevel ({Config.CurrentLevel % 10 + 1})";
    }

    void LevelTrainer()
    {
        var chapter = Config.CurrentChapter;
        if (chapter < 8)
        {
            var trainerShowed = PlayerPrefs.GetInt("TrainerShowed" + chapter);
            if (trainerShowed == 0)
            {
                if (trainerParent != null && !trainerParent.gameObject.activeSelf)
                {
                    trainerParent.SetActive(true);
                }
                var trainer = trainerList[chapter - 1];
                if (trainer != null && !trainer.gameObject.activeSelf)
                {
                    trainer.SetActive(true);
                }

                PlayerPrefs.SetInt("TrainerShowed" + chapter, 1);
            }

        }

    }
    public void LoadAvatarImage()
    {

        if (!string.IsNullOrWhiteSpace(Config.avatarImage))
        {
            string base64Image = Config.avatarImage;
            byte[] pngData = System.Convert.FromBase64String(base64Image);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(pngData);
            avatarRawImage.texture = texture;
        }

    }

    void CheckTime()
    {
        if (gameWinPanel.gameObject.activeSelf)
        {
            timeText.gameObject.SetActive(false);
            timeBox.SetActive(false);
            return;

        }
        if (gameoverPanel.gameObject.activeSelf)
        {
            timeText.gameObject.SetActive(false);
            timeBox.SetActive(false);
            return;

        }
        if (Config.CurrentChapterDifficult > 4)
        {
            if (startTimer)
            {

                timeText.text = ((int)time).ToString();
                if (((int)time) > 0)
                {
                    time -= Time.deltaTime;
                }
                if (((int)time) == 0)
                {
                    timeText.gameObject.SetActive(false);
                    timeBox.SetActive(false);



                    GameOver();
                    return;

                }
            }
        }

    }
    void GameOver()
    {
        gameoverPanel.SetActive(true);
        gameStartPanel.SetActive(false);

        //pauseButton.SetActive(false);
        PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 1);
        //mainCamera.SetActive(false);
        SkillControl.UpdateSkill();

    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }


    void Update()
    {
        if (pausePanel.gameObject.activeSelf)
        {
            Pause();
        }
        else
        {
            Resume();
        }

        bool hasFinished = animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        if (hasFinished)
        {
            PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 1);
            CheckTime();
            skipControl.SetActive(false);
            //pauseButton.SetActive(true);
            gameStartPanel.SetActive(true);

            if (gameWinPanel.activeSelf || gameoverPanel.activeSelf)
            {
                gameStartPanel.SetActive(false);

            }


            if (Config.CurrentChapterDifficult > 4)
            {
                startTimer = true;
                timeText.gameObject.SetActive(true);
                timeBox.SetActive(true);

            }
            //GoToEndOfAnimation();
        }




        player2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.05f, player.transform.position.z);

    }

    public void ClickSkipButton()
    {
        PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 1);
        GoToEndOfAnimation();

    }



    /// Animasyonda son kareye git
    public void GoToEndOfAnimation()
    {
        if (PlayerPrefs.GetInt("SkipGoToEndOfAnimation") == 1)
        {
            animator.Play(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, 1f);
            skipControl.SetActive(false);
            //pauseButton.SetActive(true);
            gameStartPanel.SetActive(true);
            PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 0);
            if (Config.CurrentChapterDifficult > 4)
            {
                startTimer = true;
                timeText.gameObject.SetActive(true);
                timeBox.SetActive(true);
            }

        }
        else
        {
            //pauseButton.SetActive(false);
            gameStartPanel.SetActive(false);
            startTimer = false;

        }


    }

}
