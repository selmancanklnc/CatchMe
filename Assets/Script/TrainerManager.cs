using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainerManager : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public GameObject player2;
    public Animator cameraAnimator;
    public GameObject step1Panel;
    public GameObject step1Objects;
    public Button step1Button;
    public GameObject step2Panel;
    public GameObject step2Objects;
    public Button step2Button;
    public GameObject step3Panel;
    public GameObject step3Objects;
    public Button step3Button;
    public GameObject gameWinPanel;
    private bool hasAnimanedFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        step1Button.onClick.AddListener(Step1Clicked);
        step2Button.onClick.AddListener(Step2Clicked);
        step3Button.onClick.AddListener(Step3Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        bool hasFinished = cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        if (hasFinished && !hasAnimanedFinished)
        {
            step1Panel.SetActive(true);
            step1Objects.SetActive(true);
            Time.timeScale = 0;
            hasAnimanedFinished = true;

        }
        player2.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.05f, player.transform.position.z);

    }

    async void Step1Clicked()
    {
        Time.timeScale = 1;
        step1Panel.SetActive(false);
        step1Objects.SetActive(false);

        var currentFloor = GameObject.Find($"floor42");
        currentFloor.SetActive(false);
        var nextFloor = GameObject.Find($"floor40");

        await MoveToTargetPosition(nextFloor.transform.position, 0.5f);
        await Task.Delay(500);

        step2Panel.SetActive(true);
        //step2Objects.SetActive(true);
        //Time.timeScale = 0;

        //hasFinished = true;
    }
    async void Step2Clicked()
    {
        Time.timeScale = 1;
        step2Panel.SetActive(false);
        step2Objects.SetActive(false);

        var currentFloor = GameObject.Find($"floor49");
        currentFloor.SetActive(false);
        var nextFloor = GameObject.Find($"floor31");

        await MoveToTargetPosition(nextFloor.transform.position, 0.5f);
        await Task.Delay(500);

        step3Panel.SetActive(true);
        step3Objects.SetActive(true);
        Time.timeScale = 0;

        //hasFinished = true;
    }
    void Step3Clicked()
    {

        Time.timeScale = 1;
        step3Panel.SetActive(false);
        step3Objects.SetActive(false);
        gameWinPanel.SetActive(true);
        animator.Play("defeat");
        //gamewin
    }


    public void GameWin()
    {
        SceneManager.LoadScene(2);

    }
    async Task MoveToTargetPosition(Vector3 target, float duration)
    {

        Vector3 startPosition = player.transform.position;
        float angel = 0;
        if ((int)target.x == (int)startPosition.x)
        {
            angel = target.z < startPosition.z ? 180 : 0;
        }
        else if ((int)target.z == (int)startPosition.z)
        {
            angel = target.x < startPosition.x ? 270 : 90;
        }
        Debug.Log(angel);
        player2.transform.rotation = Quaternion.Euler(0, angel, 0);



        //PlayerPrefs.SetInt("JumpIsActive", 1);      
        //animator.ResetTrigger("jump");
        //animator.Play("jump");


        // Animasyon geçiþini yönetmek için "isJumping" parametresini kullanýn
        animator.SetBool("isJumping", true);
        await Task.Yield(); // Bu satýr, animasyon geçiþine izin vermek için bir frame bekler


        float elapsedTime = 0;
        int frameRate = 60;
        float timePerFrame = 1f / frameRate;

        while (elapsedTime < duration)
        {
            await Task.Delay((int)(timePerFrame * 1000));
            elapsedTime += timePerFrame;
            float progress = elapsedTime / duration;

            player.transform.position = Vector3.Lerp(startPosition, target, progress);
        }

        player.transform.position = target;
        //PlayerPrefs.SetInt("JumpIsActive", 0);
        OnJumpAnimationEnd();

    }

    public void OnJumpAnimationEnd()
    {
        animator.SetBool("isJumping", false);
    }

}
