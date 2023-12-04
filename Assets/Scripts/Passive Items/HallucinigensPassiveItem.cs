using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinigensPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.currentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
