using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameisPaused = false;

    public GameObject pauseMenuUI;

    public void Start()
    {
        Time.timeScale = 1f;
        GameisPaused = false;
    }


    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }    
        } 
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;

    }

    public void LoadMenu ()
    {
        GameisPaused = false;
        SceneManager.LoadScene("StartMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

}
