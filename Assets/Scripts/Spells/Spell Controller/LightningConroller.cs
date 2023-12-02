using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : SpellController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedLightning = Instantiate(spellData.Prefab);
        spawnedLightning.transform.position = transform.position; //assign position to be same as this object, which is parented to player
        spawnedLightning.GetComponent<LightningBehavior>().DirectionChecker(castDir.ClosestEnemyDirection()); // sets direction to that of the mouse
    }

}
