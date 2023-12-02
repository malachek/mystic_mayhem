using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    //public static bool UpgradeMenuIsOpen = false;

    public GameObject upgradeMenuUI;
    public InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        //PauseMenu.GameIsPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            RandomizeUpgrades();
        }
        if (Input.GetKeyDown("m"))
        {
            OpenUpgradeMenu();
        }
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

    public void RandomizeUpgrades()
    {
        for (int i=0; i<inventoryManager.spellSlots.Count; i++)
        {
            if (inventoryManager.spellSlots[i] != null)
            {
                Debug.Log(inventoryManager.spellSlots[i]);
            }
        }
        
        Debug.Log("randomized");
    }
}
