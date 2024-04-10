using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class GetUserInformation : MonoBehaviour
{
    public TMP_Text userNameText;
    public GameObject circleBar;

    private FirebaseAuth auth;
    private FirebaseFirestore db;
    void Start()
    {


        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            //FirebaseApp.Create();
            //FetchUserInformation("your_user_id_here");



            FirebaseApp app = FirebaseApp.Create();
            auth = FirebaseAuth.GetAuth(app);
            db = FirebaseFirestore.DefaultInstance;

            if (auth.CurrentUser != null)
            {
                string userId = auth.CurrentUser.UserId;
                FetchUserInformation(userId);
            }
            else
            {
                Debug.LogError("User is not authenticated.");
            }
        });
    }

    void FetchUserInformation(string userId)
    {
        circleBar.SetActive(true); // Circle bar'ý etkinleþtir
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("users").Document(userId);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            circleBar.SetActive(false); // Circle bar'ý devre dýþý býrak
            if (task.IsFaulted)
            {
                Debug.LogError("Error fetching user information: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                string name = snapshot.GetValue<string>("username");
                string photoUrl = snapshot.GetValue<string>("userProfileUrl");


                userNameText.text = name;
                Debug.Log("Name: " + name);
                Debug.Log("PhotoUrl: " + photoUrl);
            }
            else
            {
                Debug.LogError("User document not found");
            }
        });
    }
}
