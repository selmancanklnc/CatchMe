using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;
    public Transform leaderboardContent;
    private FirebaseFirestore db;
    private string loggedInUsername;


    public TMP_Text currentUserRankText;
    public TMP_Text currentUsernameText;
    public TMP_Text currentUserScoreText;



    public GameObject loggedInUserPrefab;
    public Transform loggedInUserContent;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser != null)
            {
                loggedInUsername = auth.CurrentUser.DisplayName;
            }
            db = FirebaseFirestore.DefaultInstance;
            LoadLeaderboard();

        });


    }



    private void LoadLeaderboard()
    {
        db.Collection("users")
           .OrderByDescending("score")
           .Limit(10)
           .GetSnapshotAsync().ContinueWithOnMainThread(task => {
               QuerySnapshot snapshot = task.Result;
               int rank = 1;
               foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
               {
                   string username = documentSnapshot.GetValue<string>("username");
                   int score = documentSnapshot.GetValue<int>("score");
                   CreateLeaderboardEntry(rank, username, score, loggedInUsername);
                   rank++;
               }
           });
        LoadCurrentUserRank();
    }
    private void LoadCurrentUserRank()
    {
        db.Collection("users")
            .OrderByDescending("score")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                QuerySnapshot snapshot = task.Result;
                int rank = 1;
                bool userFound = false;
                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    string username = documentSnapshot.GetValue<string>("username");
                    int score = documentSnapshot.GetValue<int>("score");

                    if (username == loggedInUsername)
                    {
                        userFound = true;
                        currentUserRankText.text = rank.ToString();
                        currentUsernameText.text = username;
                        currentUserScoreText.text = score.ToString();
                        break;
                    }

                    rank++;
                }

                if (!userFound)
                {
                    currentUserRankText.text = "N/A";
                    currentUsernameText.text = loggedInUsername;
                    currentUserScoreText.text = "N/A";
                }
            });
    }


    private void CreateLeaderboardEntry(int rank, string username, int score, string loggedInUsername)
    {
        GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContent);
        TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();
        texts[0].text = rank.ToString();
        texts[1].text = username;
        texts[2].text = score.ToString();

        if (username == loggedInUsername)
        {
            foreach (TMP_Text text in texts)
            {
                text.color = Color.red;
            }
        }
    }
    public void UpdateLeaderboard()
    {
        // �nce leaderboardContent alt�ndaki t�m �ocuklar� silin
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Firestore'dan yeni verileri �ekin ve liderlik tablosunu yeniden olu�turun
        LoadLeaderboard();
    }
}
