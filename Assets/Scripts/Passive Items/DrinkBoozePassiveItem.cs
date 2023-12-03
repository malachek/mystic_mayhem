using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkBoozePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentPower *= 1 + passiveItemData.Multiplier / 100f;
    }
}
