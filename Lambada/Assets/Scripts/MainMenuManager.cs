using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void LeaveGame() 
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
