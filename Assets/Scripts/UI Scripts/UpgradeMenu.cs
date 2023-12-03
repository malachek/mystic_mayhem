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
        //testing only
        /*
        if (Input.GetKeyDown("space"))
        {
            RandomizeUpgrades();
        }
        if (Input.GetKeyDown("m"))
        {
            OpenUpgradeMenu();
        }
        if (Input.GetKeyDown("n"))
        {
            CloseUpgradeMenu();
        }
        */
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
        // select 4 random upgrades then call createupgradebutton with the params for the prefab
        // for (j=0; j<4; j++) choose an upgrade
        CreateUpgradeButton(upgradeFireball, 0);
    }

    // create a new upgrade button that when clicked will give the player a new ability. there are 4 upgrade positions upgrades can be displayed in (0-3)
    public GameObject upgradeFireball;
    public GameObject upgradeAK;
    public GameObject upgradeGarlic;
    private void CreateUpgradeButton(GameObject buttonPrefab, int upgradePosition)
    {
        GameObject upgradeButton = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        upgradeButton.transform.SetParent(upgradeMenuUI.transform);
        upgradeButton.transform.localScale = new Vector3(1, 1, 1);
        var rectTransform = upgradeButton.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        rectTransform.anchorMin = new Vector2(0.05f, upgradePosition*0.2f);
        rectTransform.anchorMax = new Vector2(0.95f, upgradePosition*0.2f + 0.2f);
    }
}
