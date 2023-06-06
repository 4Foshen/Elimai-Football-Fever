using System;
using System.Collections;
using System.Linq;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    private DatabaseReference dbRef;

    [Header("Player")]
    public Text playerName;
    public Text playerPlace;
    public Text playerScore;

    [Header("Text")]
    public Text LeaderBoardText;
    public Text LeaderBoardScore;

    [Header("GameObjects")]
    [SerializeField] private GameObject noWifi;
    [SerializeField] private GameObject loginScreen;
    


    [NonSerialized] public int highScore = 0;
    [NonSerialized] public string userName;
    [NonSerialized] public bool isLoaded = false;
    [NonSerialized] private string key;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("InternetConnection"))
            PlayerPrefs.DeleteKey("InternetConnection");
        FirebaseDatabase.GetInstance("https://elimai-football-fever-default-rtdb.firebaseio.com/");
        dbRef = FirebaseDatabase.DefaultInstance.GetReference("Leaderboard");
        if (PlayerPrefs.HasKey("Key"))
        {
            key = PlayerPrefs.GetString("Key");
            StartCoroutine(LoadData());
        }
        StartCoroutine(CheckConnection());
    }
    private void Start()
    {

        //if (highScore >= 10)
        //    playerScore.text = $"{highScore}";
        //else if (highScore < 10)
        //    playerScore.text = $"0{highScore}";
        //else
        //    playerScore.text = "";
    }
    public void AddData(string playerName, int score)
    {
        // Create a unique key for the player
        key = dbRef.Push().Key;
        PlayerPrefs.SetString("Key", key);

        // Store the player's name and score in the Firebase database
        dbRef.Child(key).Child("name").SetValueAsync(playerName);
        dbRef.Child(key).Child("score").SetValueAsync(score);
        isLoaded = true;
    }

    public void ChangeScore(int score)
    {
        dbRef.Child(key).Child("score").SetValueAsync(score);
    }
    public IEnumerator GetLeaderboard()
    {
        LeaderBoardScore.text = "";
        LeaderBoardText.text = "";
        var leaders = dbRef.OrderByChild("score").LimitToLast(5).GetValueAsync();

        yield return new WaitUntil(predicate: () => leaders.IsCompleted);

        if(leaders.Exception != null)
        {
            Debug.LogError("ERROR: " + leaders.Exception);
        }
        else if(leaders.Result.Value == null)
        {
            Debug.LogError("Result.Value == null");
        }
        else
        {
            DataSnapshot snapshot = leaders.Result;
            foreach(DataSnapshot dataChildSnapshot in snapshot.Children.Reverse())
            {
                string snapshotScore = dataChildSnapshot.Child("score").Value.ToString();
                if (Convert.ToInt16(snapshotScore) >= 10)
                    LeaderBoardScore.text += dataChildSnapshot.Child("score").Value.ToString() + "\n";
                else
                    LeaderBoardScore.text += "0" + dataChildSnapshot.Child("score").Value.ToString() + "\n";
                LeaderBoardText.text += dataChildSnapshot.Child("name").Value.ToString() + "\n";
            }
        }
    }
    public IEnumerator GetRank()
    {
        var players = dbRef.OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => players.IsCompleted);

        if (players.Exception != null)
        {
            Debug.LogError("ERROR: " + players.Exception);
        }
        else if (players.Result.Value == null)
        {
            Debug.LogError("Result.Value == null");
        }
        else
        {
            DataSnapshot snapshot = players.Result;
            int rank = 1;

            foreach(DataSnapshot dataChildSnapshot in snapshot.Children.Reverse())
            {
                int score = int.Parse(dataChildSnapshot.Child("score").Value.ToString());
                if (score <= highScore)
                {
                    playerPlace.text = rank.ToString();
                    break;
                }
                rank++;
            }
        }
    }
    public IEnumerator LoadData()
    {
        var user = dbRef.Child(key).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);
        if (user.Exception != null)
        {
            Debug.LogError("ERROR: " + user.Exception);
        }
        else if (user.Result.Value == null)
        {
            Debug.LogError("Result.Value == null");
        }
        else
        {
            DataSnapshot snapshot = user.Result;
            userName = snapshot.Child("name").Value.ToString();
            string dataScore = snapshot.Child("score").Value.ToString();
            Debug.Log(dataScore);
            highScore = Convert.ToInt16(dataScore);
            Debug.Log(highScore);
            Debug.Log(userName);
            playerName.text = userName;
            if (highScore >= 10)
                playerScore.text = $"{highScore}";
            else
                playerScore.text = $"0{highScore}";
            isLoaded = true;
            noWifi.SetActive(false);
            Debug.Log("Database loading: Completed");
        }
    }
    public IEnumerator CheckConnection()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Connection is failed");
            noWifi.SetActive(true);
            loginScreen.SetActive(false);
        }
        else
        {
            noWifi.SetActive(false);
            Debug.Log("Connection is successfully");
        }
    }
}