using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<SpellController> spellSlots = new List<SpellController>(6);
    public int[] spellLevels = new int[6];
    public List<Image> spellUISlots = new List<Image>();
    //public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    //public int[] spellLevels = new int[6];
    //passive items have not been made yet, but this is how they would be implemented here
    //  at least for the start of their implementation


    public void AddSpell(int slotIndex, SpellController spell) //Add a spell to a specific slot
    {
        spellSlots[slotIndex] = spell;
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
}
