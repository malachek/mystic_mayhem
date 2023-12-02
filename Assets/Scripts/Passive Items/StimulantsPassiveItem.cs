using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimulantsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMaxDashCooldown *= passiveItemData.Multiplier / 100;
    }
}
