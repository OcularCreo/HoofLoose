using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCMenu : MonoBehaviour
{
    public static ESCMenu instance;
    public GameObject pauseMenu;
    public static bool isPaused;

    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject settingsUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused) 
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void PauseGame() 
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        mainUI.SetActive(true);
        settingsUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        //Debug.Log("Quit Game");
        ResumeGame();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
