using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject loginScreen;
    public GameObject leaderBoard;
    public GameObject about;
    public GameObject ruButton;
    public GameObject kzButton;
    public GameObject ruButtonPlay;
    public GameObject kzButtonPlay;
    [Header("Sound")]
    public AudioSource music;
    public AudioSource sound;
    public AudioClip clickSound;
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject soundOn;
    public GameObject soundOff;
    [Header("Text")]
    public Text leaderText;
    public Text aboutText;
    public GameObject attentionText;
    [Header("Animator")]
    public Animator anim;
    [Header("Data Base")]
    public DataBase db;

    private string language = "ru";
    private string userName;



    private void Start()
    {
        if (PlayerPrefs.HasKey("Key"))
        {
            loginScreen.SetActive(false);
        }
        if (PlayerPrefs.HasKey("Sound Off"))
            if (PlayerPrefs.GetInt("Sound Off") == 1)
                SoundOff();
        if (PlayerPrefs.HasKey("Music Off"))
            if (PlayerPrefs.GetInt("Music Off") == 1)
                MusicOff();
        if (PlayerPrefs.HasKey("isLeaderBoard"))
            if (PlayerPrefs.GetInt("isLeaderBoard") == 1)
            {
                OpenLeaderBoard();
                PlayerPrefs.DeleteKey("isLeaderBoard");
            }
        if (PlayerPrefs.HasKey("Language"))
            if (PlayerPrefs.GetString("Language") == "ru")
                Russian();
        if (PlayerPrefs.GetString("Language") == "kz")
            Kazakh();
    }
    public void OpenLeaderBoard()
    {
        about.SetActive(false);
        leaderBoard.SetActive(true);

        StartCoroutine(db.GetLeaderboard());
        StartCoroutine(db.GetRank());
    }
    public void OpenAbout()
    {
        about.SetActive(true);
        leaderBoard.SetActive(false);
    }
    public void Close()
    {
        about.SetActive(false);
        leaderBoard.SetActive(false);
        db.LeaderBoardText.text = "";
        db.LeaderBoardScore.text = "";
    }
    public void ChangeLanguage()
    {
        // Из русского в казахский, и наоборот
        if (language == "ru")
            Kazakh();
        else if (language == "kz")
            Russian();
    }
    public void MusicOn()
    {
        PlayerPrefs.SetInt("Music Off", 0);
        music.enabled = true;
        musicOn.SetActive(true);
        musicOff.SetActive(false);
    }
    public void MusicOff()
    {
        PlayerPrefs.SetInt("Music Off", 1);
        music.enabled = false;
        musicOn.SetActive(false);
        musicOff.SetActive(true);
    }
    public void SoundOn()
    {
        PlayerPrefs.SetInt("Sound Off", 0);
        sound.enabled = true;
        soundOn.SetActive(true);
        soundOff.SetActive(false);
    }
    public void SoundOff()
    {
        PlayerPrefs.SetInt("Sound Off", 1);
        sound.enabled = false;
        soundOn.SetActive(false);
        soundOff.SetActive(true);
    }
    public void PlaySound()
    {
        sound.PlayOneShot(clickSound);
    }
    public void Russian()
    {
        anim.SetBool("language", false);
        aboutText.text = "Разработчики: ";
        leaderText.text = "Таблица лидеров";
        ruButtonPlay.SetActive(true);
        kzButtonPlay.SetActive(false);
        ruButton.SetActive(true);
        kzButton.SetActive(false);
        language = "ru";
        PlayerPrefs.SetString("Language", language);
    }
    public void Kazakh()
    {
        anim.SetBool("language", true);
        aboutText.text = "Әзірлеушелер: ";
        leaderText.text = "Лидерлер тақтасы";
        ruButtonPlay.SetActive(false);
        kzButtonPlay.SetActive(true);
        ruButton.SetActive(false);
        kzButton.SetActive(true);
        language = "kz";
        PlayerPrefs.SetString("Language", language);
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/fc_elimai/");
    }

    public void InputNickname(string input)
    {
        userName = input;
        Debug.Log(userName);
    }
    public void Apply()
    {
        if (userName == null)
        {
            attentionText.SetActive(true);
        }
        else 
        { 
        //PlayerPrefs.SetString("Username", userName);
        db.playerName.text = userName;
        db.AddData(userName, 0);
        db.playerScore.text = "00";
        loginScreen.SetActive(false);
        }
    }
}