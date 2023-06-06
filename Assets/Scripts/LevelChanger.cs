using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private Animator animator;
    private GameObject dataBase;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        dataBase = GameObject.FindGameObjectWithTag("DataBase");
    }
    public void LoadGame()
    {
        DontDestroyOnLoad(dataBase);
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMenu()
    {
        DontDestroyOnLoad(dataBase);
        SceneManager.LoadScene("MainMenu");
    }
    
    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }

    public void FadeInMenu()
    {
        animator.SetTrigger("FadeInMenu");
    }
}
