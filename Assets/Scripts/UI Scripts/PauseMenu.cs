using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    

    public GameObject pauseMenuUI;

    public string MainMenu = "MainMenu";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseManager.GameIsPaused)
            {
                PauseManager.Unpause();
                pauseMenuUI.SetActive(false);
            }
            else
            {
                PauseManager.Pause();
                pauseMenuUI.SetActive(true);
            }
        }
    }

    public void OpenPauseMenu()
    {
        PauseManager.Pause();
        pauseMenuUI.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        PauseManager.Unpause();
        pauseMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        PauseManager.Unpause();
        pauseMenuUI.SetActive(false);
        //Application.Quit();
        SceneManager.LoadScene(MainMenu);
    }
}
