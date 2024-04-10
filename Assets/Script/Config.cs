using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Config
{
    public static int UserLevel = 0;
    public static int CurrentLevel = 0;
    public static int Score = 0;
    public static int avatarIndex = 0;
    public static string userId = "";
    public static string playerName = "";
    public static string playerImageURL = "";
    public static string avatarImage = "";
    public static List<GameObject> closedObjects = new List<GameObject>();
    public static int CurrentChapter => CurrentLevel / 10 + 1;
    public static int UserChapter => UserLevel / 10 + 1;
    public static int ColAndRowCount => CurrentChapter < 7 ? 9 : 7;
    public static int CurrentChapterDifficult => CurrentChapter > 6 ? (CurrentChapter) - 6 : CurrentChapter;

    public static void ChangeLevel()
    {
        CurrentLevel++;

        if (CurrentLevel > UserLevel)
        {
          
            UserLevel = CurrentLevel;
            var levelPoint = PlayerPrefs.GetInt("LevelPoint", 10);
            Score = Score + levelPoint;

            PlayerPrefs.SetInt("LevelPoint", 10);
            FirestoreExample.UpdateScore();
        }
    }
    public static void ChangeLevelPoint()
    {
        if (CurrentLevel == UserLevel)
        {
            var levelPoint = PlayerPrefs.GetInt("LevelPoint", 10);
            levelPoint--;
            if (levelPoint < 2)
            {
                levelPoint = 1;
            }
            PlayerPrefs.SetInt("LevelPoint", levelPoint);

        }

    }

    public static void SaveAvatarIndex(string userId, int index)
    {
        DocumentReference userDocRef = FirebaseFirestore.DefaultInstance.Collection("users").Document(userId);
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "avatarIndex", index }
        };
        userDocRef.UpdateAsync(data);
        avatarIndex = index;
    }

    public static async Task<int> GetAvatarIndex(string userId)
    {
        DocumentReference userDocRef = FirebaseFirestore.DefaultInstance.Collection("users").Document(userId);
        DocumentSnapshot snapshot = await userDocRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.GetValue<int>("avatarIndex");
        }
        else
        {
            return 0;
        }
    }
}

public class Inventory
{

    public static int Coin = 100;
    public static int MeteorSkill = 0;
    public static int OneHitSkill = 0;
    public static int ChangeReailtySkill = 0;
    public static int TimeShiftSkill = 0;
}