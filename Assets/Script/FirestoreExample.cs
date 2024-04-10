using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirestoreExample
{




    //public static void GetUser2()
    //{
    //    FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //    DocumentReference userRef = db.Collection("users").Document("OCYqXlKAVHW6qEYd0udYZz3OdI73");
    //    userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("Error fetching user information: " + task.Exception);
    //            return;
    //        }

    //        DocumentSnapshot snapshot = task.Result;
    //        if (snapshot.Exists)
    //        {
    //            Config.userId = snapshot.GetValue<string>("userId");
    //            Config.playerName = snapshot.GetValue<string>("username");
    //            Config.playerImageURL = snapshot.GetValue<string>("userProfileUrl");
    //            Config.UserLevel = snapshot.GetValue<int>("userLevel");
    //            Config.CurrentLevel = snapshot.GetValue<int>("userLevel");
    //            Config.Score = snapshot.GetValue<int>("score");


    //            Inventory.Coin = snapshot.GetValue<int>("coin");
    //            Inventory.OneHitSkill = snapshot.GetValue<int>("oneHitSkill");
    //            Inventory.MeteorSkill = snapshot.GetValue<int>("meteorSkill");
    //            Inventory.TimeShiftSkill = snapshot.GetValue<int>("timeShiftSkill");
    //            Inventory.ChangeReailtySkill = snapshot.GetValue<int>("changeReailtySkill");

    //        }
    //    });


    //}


    public static void GetUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        if (auth?.CurrentUser != null)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference userRef = db.Collection("users").Document(auth.CurrentUser.UserId);
            userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error fetching user information: " + task.Exception);
                    return;
                }

                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Config.userId = snapshot.GetValue<string>("userId");
                    Config.playerName = snapshot.GetValue<string>("username");
                    Config.playerImageURL = snapshot.GetValue<string>("userProfileUrl");
                    Config.UserLevel = snapshot.GetValue<int>("userLevel");
                    Config.CurrentLevel = snapshot.GetValue<int>("userLevel");
                    Config.Score = snapshot.GetValue<int>("score");
                    Config.avatarIndex = snapshot.GetValue<int>("avatarIndex");


                    Inventory.Coin = snapshot.GetValue<int>("coin");
                    Inventory.OneHitSkill = snapshot.GetValue<int>("oneHitSkill");
                    Inventory.MeteorSkill = snapshot.GetValue<int>("meteorSkill");
                    Inventory.TimeShiftSkill = snapshot.GetValue<int>("timeShiftSkill");
                    Inventory.ChangeReailtySkill = snapshot.GetValue<int>("changeReailtySkill");

                }
            });

        }

    }


    public static void UpdateScore()
    {
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference userRef = db.Collection("users").Document(auth.CurrentUser.UserId);

            Dictionary<string, object> userData = new Dictionary<string, object>
        {
           { "userLevel",Config.UserLevel },
           { "score",Config.Score }
         };

            userRef.UpdateAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error saving user to Firestore: " + task.Exception);
                    return;
                }

                Debug.Log("User saved to Firestore");
            });

        }

    }


    public static void UpdateSkill()
    {
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference userRef = db.Collection("users").Document(auth.CurrentUser.UserId);

            Dictionary<string, object> userData = new Dictionary<string, object>
        {
         { "oneHitSkill",Inventory.OneHitSkill },
         { "meteorSkill",Inventory.MeteorSkill },
         { "timeShiftSkill",Inventory.TimeShiftSkill },
         { "changeReailtySkill",Inventory.ChangeReailtySkill },
         { "coin",Inventory.Coin },
         };

            userRef.UpdateAsync(userData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error saving user to Firestore: " + task.Exception);
                    return;
                }

                Debug.Log("User saved to Firestore");
            });

        }

    }

}
