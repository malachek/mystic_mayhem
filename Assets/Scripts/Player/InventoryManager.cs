using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<SpellController> spellSlots = new List<SpellController>(6);
    public int[] spellLevels = new int[6];
    //public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    //public int[] spellLevels = new int[6];
    //passive items have not been made yet, but this is how they would be implemented here
    //  at least for the start of their implementation


    public void AddSpell(int slotIndex, SpellController spell) //Add a spell to a specific slot
    {
        spellSlots[slotIndex] = spell;
    }

    public void LevelUpSpell(int slotIndex)
    {
        
    }
}
