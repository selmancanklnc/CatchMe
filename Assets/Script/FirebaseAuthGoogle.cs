using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using GooglePlayGames;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirebaseAuthGoogle : MonoBehaviour
{
    public GameObject signInPanel;
    public GameObject loginButtonIos;
    public GameObject netControlPanel;
    public AudioListener audiolistener;
    public GameObject circleBar;
    public GameObject exitPanel;
    private bool netcontrolconnected = true;



    void Start()
    {  //  NetControl();

        Login(); 
    }

    void Login()
    {
        // Android cihazdaysak Android butonunu etkinleþtir
        if (Application.platform == RuntimePlatform.Android)
        {
            circleBar.SetActive(true);


            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false /* Don't force refresh */).Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            // Otomatik oturum açma iþlemini baþlatýn
            SignIn();
            circleBar.SetActive(false);

        }
        // Ios cihazdaysak Ios butonunu etkinleþtir
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            circleBar.SetActive(true);
            SignIn();
            circleBar.SetActive(false);
        }

    }
    private void SignIn()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Google oturum açma baþarýlý.");
                if (Application.platform == RuntimePlatform.Android)
                {
                    FirebaseSignIn();

                }
                // Ios cihazdaysak Ios butonunu etkinleþtir
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {

                    FirebaseSignInIphone();
                }
            }
            else
            {

                Debug.LogError("Google oturum açma baþarýsýz.");
            }
        });
    }

    private void FirebaseSignIn()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string idToken = PlayGamesPlatform.Instance.GetServerAuthCode();
        Credential credential = PlayGamesAuthProvider.GetCredential(idToken);
        Debug.Log("token2:" + idToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Firebase oturum açma iptal edildi.");

                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase oturum açma baþarýsýz: " + task.Exception);

                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("Signed in with Google: " + newUser.DisplayName);
            OnUserSignedIn(newUser);

        });
    }


    private void FirebaseSignInIphone()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        GameCenterAuthProvider.GetCredentialAsync().ContinueWithOnMainThread(f =>
       {
           auth.SignInAndRetrieveDataWithCredentialAsync(f.Result).ContinueWithOnMainThread(task =>
           {
               if (task.IsCanceled)
               {
                   Debug.LogError("Firebase oturum açma iptal edildi.");

                   return;
               }
               if (task.IsFaulted)
               {
                   Debug.LogError("Firebase oturum açma baþarýsýz: " + task.Exception);

                   return;
               }

               FirebaseUser newUser = task.Result.User;
               Debug.Log("Signed in with Google: " + newUser.DisplayName);
               OnUserSignedIn(newUser);

           });
       });

    }



    public void NetControl()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!netControlPanel.gameObject.activeSelf)
            {
                netControlPanel.SetActive(true);

            }
            AudioListener.volume = 0;
            netcontrolconnected = false;
        }
        else
        {
            if (netControlPanel.gameObject.activeSelf)
            {
                netControlPanel.SetActive(false);
            }
            AudioListener.volume = 1;
            if (!netcontrolconnected)
            {
                Login();
            }
            netcontrolconnected = true;

        }

    }


    //Firestore'a Yeni kullanýcýyý kaydet.
    void SaveUserToFirestore(FirebaseUser user)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("users").Document(user.UserId);
        int userLevel = 0;
        int score = 0;
        int oneHitSkill = 0;
        int meteorSkill = 0;
        int timeShiftSkill = 0;
        int changeReailtySkill = 0;
        int coin = 100;
        int avatarIndex = 0;

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching user information: " + task.Exception);
                //popupPanel.SetActive(true);
                //popupText.text = "Error fetching user information: " + task.Exception;
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                userLevel = snapshot.GetValue<int>("userLevel");
                score = snapshot.GetValue<int>("score");
                oneHitSkill = snapshot.GetValue<int>("oneHitSkill");
                meteorSkill = snapshot.GetValue<int>("meteorSkill");
                timeShiftSkill = snapshot.GetValue<int>("timeShiftSkill");
                changeReailtySkill = snapshot.GetValue<int>("changeReailtySkill");
                coin = snapshot.GetValue<int>("coin");
                avatarIndex = snapshot.GetValue<int>("avatarIndex");
            }
            Dictionary<string, object> userData = new Dictionary<string, object>
                {
                    { "userId", user.UserId },
                    { "username", user.DisplayName },
                    { "userProfileUrl", user.PhotoUrl.ToString() },
                    { "userLevel",userLevel },
                    { "score",score },
                    { "oneHitSkill",oneHitSkill },
                    { "meteorSkill",meteorSkill },
                    { "timeShiftSkill",timeShiftSkill },
                    { "changeReailtySkill",changeReailtySkill },
                    { "coin",coin },
                    { "avatarIndex", avatarIndex }


                };

            userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error saving user to Firestore: " + task.Exception);
                    return;
                }
                SceneManager.LoadScene(1);
                Debug.Log("Firebase oturum açýldý.");
                Debug.LogFormat("Firebase oturum açma baþarýlý: {0} ({1})", user.DisplayName, user.UserId);
                Debug.Log("User saved to Firestore");

            });
        });


    }

    void OnUserSignedIn(FirebaseUser user)
    {

        SaveUserToFirestore(user);

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //if (logoutePanel.activeSelf)

            //{
            //    logoutePanel.SetActive(false);
            //}
            //else
            //{


            //}

            if (exitPanel)
            {
                exitPanel.SetActive(true);
            }
        }

        NetControl();

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