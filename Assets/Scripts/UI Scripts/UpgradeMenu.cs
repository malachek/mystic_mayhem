using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;



public class UpgradeMenu : MonoBehaviour
{
    public GameObject upgradeMenuUI;
    public GameObject upgradePanelUI;
    public InventoryManager inventoryManager;
    public CharacterStats characterStats;

    /*
    ATTENTION
    make sure that when a new upgrade is added to the game is in the "possible spells/passives" list or else it will not be able to show up in the upgrade menu.
    in addition make sure that you create a button gameobject prefab for the upgrade that can be displayed in the menu.

    also spawning the buttons in is hard coded for now (in populateupgradepanel)

    To add a new spell to the upgrade menu:
    1. Open the scene view to canvas/UI/upgrademenu/panel
    2. Go to assets/prefabs/UI then select either the spells or passive items folder
    3. Drag a prefab into the scene view as a child of "panel"
    4. Modify the prefab in the scene view (rename, change description/image) then drag it back into the folder. select "original prefab"
    5. At the top of this script, create a new gameobject called "upgradeButton" followed by the upgrade name. Create this under tha "Spell Upgrade Buttons" or "Passive Item Upgrade Buttons" header
    6. In this script in the "PopulateUpgradePanel" function, add an else if statement for the new upgrade. change the "equal to" string to the first few characters of the spell/passive prefab you made in step 4 and change the name of the upgrade button being created to the one you made in step 5.
    7. At the bottom of this script, create a new gameobject with the name of the spell/passive you created followed by "prefab." Create this under the "Spell Controller Prefabs" or "Item Controller Prefabs" header
    8. Below the game object, create a new function with the same name as the new spell/passive. After copying the function, changed the search string to the name of the spell/passive controller's first level upgrade. change the spawned spell to the prefab gameobject you created in step 7.
    9. In the scene view, navigate to assets/prefabs/UI and select the UI prefab in that folder. Scroll down to the upgrade menu script. Under possible spells/passive items, create a new field. Drag the prefab for the spell/passive controller's first level upgrade into this field.
    10. In the UI prefab, navigate to "spell upgrade buttons" or "passive item upgrade buttons." Drag the button you created in step 4 into this field. 
    11. In the UI prefab, navigate to "spell controller prefabs" or "passive item controller prefabs." drag the spell/passive controller's first level upgrade into the appropraite field
    12. Under assets/prefabs/ui, find the button you created. Under "on click" event, create two fields. Drag the UI prefab under assets/prefabs/UI into both. For the first, select UpgradeMenu/(the name of the spell). For the second, select UpgradeMenu/CloseUpgradeMenu
    13. To test if the upgrade is working properly, comment in the update function but make sure to comment it back out when done
    */

    // possible upgrades
    // spells
    public List<SpellController> possibleSpells = new List<SpellController>(9);
    // passive abilities
    public List<PassiveItem> possiblePassiveItems = new List<PassiveItem>(9);

    // 5. upgrade button
    [Header("Spell Upgrade Buttons")]
    public GameObject upgradeButtonAk;
    public GameObject upgradeButtonFireball;
    public GameObject upgradeButtonGarlic;
    public GameObject upgradeButtonIce;
    public GameObject upgradeButtonLightning;
    [Header("Passive Item Upgrade Buttons")]
    public GameObject upgradeButtonBulkup;
    public GameObject upgradeButtonDrinkbooze;
    public GameObject upgradeButtonPutonmorerobes;
    public GameObject upgradeButtonStimulants;


    
    void Update()
    {
        //testing only
        
        
        if (Input.GetKeyDown("space"))
        {
            RandomizeUpgrades();
        }
        if (Input.GetKeyDown("m"))
        {
            OpenUpgradeMenu();
        }
        
        if (Input.GetKeyDown("x"))
        {
            CloseUpgradeMenu();
        }
        
    }


    // opens the upgrade menu which is closed automatically after an ugrade is selected
    public void OpenUpgradeMenu()
    {
        RandomizeUpgrades();
        PauseManager.Pause();
        upgradeMenuUI.SetActive(true);
    }
    public void CloseUpgradeMenu()
    {
        PauseManager.Unpause();
        upgradeMenuUI.SetActive(false);
    }

    // selects 4 random upgrades to show up in the upgrade panel
    public void RandomizeUpgrades()
    {
        ClearUpgradePanel();
        UpdatePossibleUpgrades();

        int numUpgradeButtons = 4;
        if (RemainingUpgrades() < 4)
        {
            numUpgradeButtons = RemainingUpgrades();
        }

        // create a list of upgrades (strings) that will be added to the upgrade menu
        // Note: this section has been hard coded and should be refactored to work directly from
        List<string> spellUpgradeNames = new List<string>();
        List<string> passiveItemUpgradeNames = new List<string>();

        spellUpgradeNames.Clear();
        passiveItemUpgradeNames.Clear();

        System.Random rd = new System.Random();
        // get the name of a valid upgrade
        for (int i=0; i < numUpgradeButtons; i++)
        {
            int rand_num = rd.Next(0, 2);
            if (possibleSpells == null) { rand_num = 1; Debug.Log("null"); }
            if (possiblePassiveItems == null) { rand_num = 0; Debug.Log("null"); }
            if (rand_num == 0)
            {
                string spellName = null;
                while (spellName == null)
                {
                    spellName = possibleSpells[rd.Next(0, possibleSpells.Count)].name;
                    if (spellUpgradeNames.Contains(spellName))
                    {
                        spellName = null;
                    }
                }
                spellUpgradeNames.Add(spellName);
            }
            if (rand_num == 1)
            {
                string passiveItemName = null;
                while (passiveItemName == null)
                {
                    passiveItemName = possiblePassiveItems[rd.Next(0, possiblePassiveItems.Count)].name;
                    if (passiveItemUpgradeNames.Contains(passiveItemName))
                    {
                        passiveItemName = null;
                    }
                }
                passiveItemUpgradeNames.Add(passiveItemName);
            }
        }

        PopulateUpgradePanel(spellUpgradeNames, passiveItemUpgradeNames);
    }

    // ive llost it. just hard coded for now. make sure that new upgrades have a button.
    // 6. add upgrade to panel
    private void PopulateUpgradePanel(List<string> spells, List<string> passives)
    {
        int pos = 0;
        foreach (string spell in spells)
        {
            //Debug.Log(spell);
            if (spell.Substring(0, 10) == "AK Control") { CreateUpgradeButton(upgradeButtonAk, pos); pos++; }
            else if (spell.Substring(0, 10) == "Fireball C") { CreateUpgradeButton(upgradeButtonFireball, pos); pos++; }
            else if (spell.Substring(0, 10) == "Garlic Con") { CreateUpgradeButton(upgradeButtonGarlic, pos); pos++; }
            else if (spell.Substring(0, 10) == "Ice Contro") { CreateUpgradeButton(upgradeButtonIce, pos); pos++; }
            else if (spell.Substring(0, 10) == "Lightning ") { CreateUpgradeButton(upgradeButtonLightning, pos); pos++; }
        }
        foreach (string passive in passives)
        {
            //Debug.Log(passive);
            if (passive.Substring(0, 8) == "Bulk Up ") { CreateUpgradeButton(upgradeButtonBulkup, pos); pos++; }
            else if (passive.Substring(0, 8) == "Drink Bo") { CreateUpgradeButton(upgradeButtonDrinkbooze, pos); pos++; }
            else if (passive.Substring(0, 8) == "Put On M") { CreateUpgradeButton(upgradeButtonPutonmorerobes, pos); pos++; }
            else if (passive.Substring(0, 8) == "Stimulan") { CreateUpgradeButton(upgradeButtonStimulants, pos); pos++; }
        }
    }

    private void ClearUpgradePanel()
    {
        foreach (Transform child in upgradePanelUI.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private int RemainingUpgrades()
    {
        int remainingUpgrades = 0;
        for (int i=0; i<possibleSpells.Count; i++)
        {
            if (possibleSpells[i] != null)
            {
                remainingUpgrades++;
            }
        }
        for (int i = 0; i < possiblePassiveItems.Count; i++)
        {
            if (possiblePassiveItems[i] != null)
            {
                remainingUpgrades++;
            }
        }
        return remainingUpgrades;
    }

    // by default, every level 1 upgrade will show up in the pool of upgrades. this function checks if the current level of a spell or passive is possessed by the player
    // if they already have the spell/passive, the next level of the spell/passive replaces it. set to null if no next level is found.
    // also this can only increase the level of possible upgrades by 1, so if the player somehow starts with a level 3 ability, the level 1 in possible upgrades only increments once per call.
    public void UpdatePossibleUpgrades()
    {
        UpdatePossibleSpells();
        UpdatePossiblePassiveItems();
    }

    // update possible spells
    private void UpdatePossibleSpells()
    {
        for (int i = 0; i < possibleSpells.Count; i++)
        {
            for (int j = 0; j < inventoryManager.spellSlots.Count; j++)
            {
                //var prefabGameObject = PrefabUtility.GetPrefabInstanceHandle(inventoryManager.spellSlots[1]);
                //Debug.Log(prefabGameObject);
                // im trying to find a way to test if a prefab is a clone. this is super scuffed, so rn im taking the name
                // of the spell and just comparing it. not good. 

                // the player already has the spell. put the upgraded version of the spell into the upgrade pool replacing the old one
                if (possibleSpells[i] != null && inventoryManager.spellSlots[j] != null)
                {
                    if (possibleSpells[i].name.Substring(0, 10) == inventoryManager.spellSlots[j].name.Substring(0, 10) && UpgradeLevelChangeNeeded(possibleSpells[i].name, inventoryManager.spellSlots[j].name))
                    {
                        //Debug.Log(possibleSpells[i]);
                        SpellController spell = possibleSpells[i];
                        if (!spell.spellData.NextLevelPrefab) //Checks if there is a next level for the current passive item
                        {
                            Debug.LogError("NO NEXT LEVEL FOR " + spell.name);
                            possibleSpells[i] = null;
                        }
                        else
                        {
                            possibleSpells[i] = spell.spellData.NextLevelPrefab.GetComponent<SpellController>();
                        }
                    }
                }
            }
        }
    }

    //update possible passives
    private void UpdatePossiblePassiveItems()
    {
        for (int i = 0; i < possiblePassiveItems.Count; i++)
        {
            for (int j = 0; j < inventoryManager.passiveItemSlots.Count; j++)
            {
                //var prefabGameObject = PrefabUtility.GetPrefabInstanceHandle(inventoryManager.spellSlots[1]);
                //Debug.Log(prefabGameObject);
                // im trying to find a way to test if a prefab is a clone. this is super scuffed, so rn im taking the name
                // of the spell and just comparing it. not good. 

                // the player already has the spell. put the upgraded version of the spell into the upgrade pool replacing the old one
                if (possiblePassiveItems[i] != null && inventoryManager.passiveItemSlots[j] != null)
                {
                    if (possiblePassiveItems[i].name.Substring(0, 10) == inventoryManager.passiveItemSlots[j].name.Substring(0, 10) && UpgradeLevelChangeNeeded(possiblePassiveItems[i].name, inventoryManager.passiveItemSlots[j].name))
                    {
                        //Debug.Log(possibleSpells[i]);
                        PassiveItem passiveItem = possiblePassiveItems[i];
                        if (!passiveItem.passiveItemData.NextLevelPrefab) //Checks if there is a next level for the current passive item
                        {
                            Debug.LogError("NO NEXT LEVEL FOR " + passiveItem.name);
                            possiblePassiveItems[i] = null;
                        }
                        else
                        {
                            possiblePassiveItems[i] = passiveItem.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>();
                        }
                    }
                }
            }
        }
    }

    // if the level of the possible spell/passive is lower or equal than the one the player has, return true.
    private bool UpgradeLevelChangeNeeded(string u1, string u2)
    {
        if (u1 == null || u2 == null)
        {
            return false;
        }
        u1 = Regex.Replace(u1, "[^0-9]", "");
        u2 = Regex.Replace(u2, "[^0-9]", "");
        int u1i = int.Parse(u1);
        int u2i = int.Parse(u2);
        // added a less than so just in case the player has a level 2 ability but the ability in the possible upgrades is level 1, it will still increase.
        if (u1i<=u2i)
        {
            return true;
        }
        return false;
    }

    // create a new upgrade button that when clicked will give the player a new ability. there are 4 upgrade positions upgrades can be displayed in (0-3)
    // when a new upgrade is added, a new button needs to be created for it.
    private void CreateUpgradeButton(GameObject buttonPrefab, int upgradePosition)
    {
        GameObject upgradeButton = Instantiate(buttonPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        upgradeButton.transform.SetParent(upgradePanelUI.transform);
        upgradeButton.transform.localScale = new Vector3(1, 1, 1);
        var rectTransform = upgradeButton.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        rectTransform.anchorMin = new Vector2(0.05f, upgradePosition*0.2f);
        rectTransform.anchorMax = new Vector2(0.95f, upgradePosition*0.2f + 0.2f);
    }

    // will be called when the upgrade button is pressed
    [Header("Spell Controller Prefabs")]
    public GameObject AKprefab;
    public void AK()
    {
        int index = inventoryManager.spellStrings.IndexOf("AK Controller Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(AKprefab);
        }
    }
    public GameObject Fireballprefab;
    public void Fireball()
    {
        int index = inventoryManager.spellStrings.IndexOf("Fireball Controller Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Fireballprefab);
        }
    }
    public GameObject Garlicprefab;
    public void Garlic()
    {
        int index = inventoryManager.spellStrings.IndexOf("Garlic Controller Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Garlicprefab);
        }
    }
    public GameObject Iceprefab;
    public void Ice()
    {
        int index = inventoryManager.spellStrings.IndexOf("Ice Controller Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Iceprefab);
        }
    }
    public GameObject Lightningprefab;
    public void Lightning()
    {
        int index = inventoryManager.spellStrings.IndexOf("Lightning Controller Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Lightningprefab);
        }
    }
    [Header("Passive Item Controller Prefabs")]
    public GameObject Bulkupprefab;
    public void Bulkup()
    {
        int index = inventoryManager.passiveItemStrings.IndexOf("Bulk Up Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Bulkupprefab);
        }
    }
    public GameObject Drinkboozeprefab;
    public void Drinkbooze()
    {
        int index = inventoryManager.passiveItemStrings.IndexOf("Drink Booze Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Drinkboozeprefab);
        }
    }
    public GameObject Putonmorerobesprefab;
    public void Putonmorerobes()
    {
        int index = inventoryManager.passiveItemStrings.IndexOf("Put On More Robes Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Putonmorerobesprefab);
        }
    }
    public GameObject Stimulantsprefab;
    public void Stimulants()
    {
        int index = inventoryManager.passiveItemStrings.IndexOf("Stimulants Level 1");
        if (index >= 0)
        {
            inventoryManager.LevelUpSpell(index);
        }
        else
        {
            characterStats.SpawnSpell(Stimulantsprefab);
        }
    }
}
