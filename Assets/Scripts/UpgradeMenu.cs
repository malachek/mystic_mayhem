using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    public static bool UpgradeMenuIsOpen = false;

    public GameObject upgradeMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        //PauseMenu.GameIsPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenUpgradeMenu()
    {
        upgradeMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseUpgradeMenu()
    {
        upgradeMenuUI.SetActive(false);
        if (PauseMenu.GameIsPaused == false)
        {
            Time.timeScale = 1f;
        }
    }
}
