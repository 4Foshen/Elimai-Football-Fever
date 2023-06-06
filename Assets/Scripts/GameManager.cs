using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerMovement player;
    public Spawner spawner;
    public Parallax parallaxBG;
    public Parallax parallaxFG;
    public FlagMovement flagMovement;
    public LevelChanger changer;
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip jumpSound;
    public AudioClip failSound;
    public AudioClip scoreSound;
    [Header("UI")]
    public Text scoreText;
    public Text scoreMenu;
    public Text highscoreText;
    public GameObject gameOver;
    public GameObject tutorial;
    public GameObject scoreBoard;

    private int score;
    private int highScore;
    private DataBase dataBase;
    [NonSerialized] public bool isPlay;

    private void Awake()
    {       
        Application.targetFrameRate = 60;
        dataBase = GameObject.FindGameObjectWithTag("DataBase").GetComponent<DataBase>();
        //if (PlayerPrefs.HasKey("Highscore"))
        //    highScore = PlayerPrefs.GetInt("Highscore");
    }
    private void Start()
    {
        StopGame();
        //Time.timeScale = 0f;
        tutorial.SetActive(true);
        if (PlayerPrefs.HasKey("Sound Off"))
            if (PlayerPrefs.GetInt("Sound Off") == 1)
                GameSoundOff();
    }
    public void Warmup()
    {
        score = 0;
        scoreText.text = score.ToString();
        StartGame();
        scoreBoard.SetActive(true);
        tutorial.SetActive(false);
    }
    public void Menu()
    {
        StopGame();
        scoreBoard.SetActive(false);
        gameOver.SetActive(true);
        scoreMenu.text = "SCORE:" + score.ToString();
        highscoreText.text = "BEST:" + dataBase.highScore.ToString();
    }
    public void Restart()
    {
        audioSource.PlayOneShot(clickSound);
        //SceneManager.LoadScene("GameScene");
        changer.FadeIn();
    }
    public void IncreaseScore()
    {
        audioSource.PlayOneShot(scoreSound);
        score++;
        scoreText.text = score.ToString();
    }
    public void GameOver()
    {
        audioSource.PlayOneShot(failSound);
        if (score > dataBase.highScore)
        {
            dataBase.highScore = score;
            if (PlayerPrefs.HasKey("Key") && dataBase.isLoaded == true)
                dataBase.ChangeScore(dataBase.highScore);
        }
        StopGame();
        Menu();
    }
    public void ToLeaderBoard()
    {
        audioSource.PlayOneShot(clickSound);
        PlayerPrefs.SetInt("isLeaderBoard", 1);
        //SceneManager.LoadScene("MainMenu");
        changer.FadeInMenu();
    }
    public void GameSoundOff()
    {
        audioSource.enabled = false;
    }

    public void StopGame()
    {
        player.enabled = false;
        //Выключаем движение флагов
        isPlay = false;
        parallaxBG.enabled = false;
        parallaxFG.enabled = false;
        spawner.enabled = false;
    }
    public void StartGame()
    {
        player.enabled = true;
        isPlay = true;
        parallaxBG.enabled = true;
        parallaxFG.enabled = true;
        spawner.enabled = true;
    }
}