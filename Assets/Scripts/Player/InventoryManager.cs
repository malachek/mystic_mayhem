using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<SpellController> spellSlots = new List<SpellController>(6);
    public List<string> spellStrings = new List<string>(6);
    public int[] spellLevels = new int[6];
    public List<Image> spellUISlots = new List<Image>(6);
    
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public List<string> passiveItemStrings = new List<string>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);


    public static InventoryManager instance;
    private void Start()
    {
        instance = this;
    }

    public void AddSpell(int slotIndex, SpellController spell) //Add a spell to a specific slot
    {
        Debug.Log(slotIndex);
        spellSlots[slotIndex] = spell;
        //spellStrings[slotIndex] = spell.name;
        spellLevels[slotIndex] = spell.spellData.Level;
        spellUISlots[slotIndex].enabled = true;
        spellUISlots[slotIndex].sprite = spell.spellData.Icon;
    }

    public void LevelUpSpell(int slotIndex)
    {
        if(spellSlots.Count > slotIndex)
        {
            SpellController spell = spellSlots[slotIndex];
            if(!spell.spellData.NextLevelPrefab) //Checks if there is a next level for the current passive item
            {
                Debug.LogError("NO NEXT LEVEL FOR " + spell.name);
                return;
            }
            GameObject upgradedSpell = Instantiate(spell.spellData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedSpell.transform.SetParent(transform); //Set the spell to be a child of the player
            AddSpell(slotIndex, upgradedSpell.GetComponent<SpellController>());
            Destroy(spell.gameObject);
            spellLevels[slotIndex] = upgradedSpell.GetComponent<SpellController>().spellData.Level; //To make sure we have the correct weapon level
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) //Add a spell to a specific slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        //passiveItemStrings[slotIndex] = passiveItem.name;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab) //Checks if there is a next level for the current passive item
            {
                Debug.LogError("NO NEXT LEVEL FOR " + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform); //Set the passive item to be a child of the player
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; //To make sure we have the correct passive item level
        }
    }
}
