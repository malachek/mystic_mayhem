using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkUpPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        int healthAdded = (int)(player.currentMaxHealth * passiveItemData.Multiplier / 100);
        player.currentMaxHealth += healthAdded;
        player.currentHealth += healthAdded;
    }
}
