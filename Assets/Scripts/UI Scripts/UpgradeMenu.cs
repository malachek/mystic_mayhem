using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    //public static bool UpgradeMenuIsOpen = false;

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

    public void OpenUpgradeMenu()
    {
        PauseManager.Pause();
        upgradeMenuUI.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        PauseManager.Unpause();
        upgradeMenuUI.SetActive(false);
    }
}
