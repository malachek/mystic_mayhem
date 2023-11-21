using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenuUI;
    public GameObject winMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [HideInInspector]
    public bool isOpened;
    public void OpenWinMenu()
    {
        isOpened = true;
        isOpened = true;
        PauseManager.Pause();
        winMenuUI.SetActive(true);
    }
    public void OpenDeathMenu()
    {
        isOpened = true;
        PauseManager.Pause();
        deathMenuUI.SetActive(true);
    }

    public void RestartScene()
    {
        PauseManager.Unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
