using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOnMoreRobesPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentArmor *= 1 + passiveItemData.Multiplier / 100f;
    }
}
