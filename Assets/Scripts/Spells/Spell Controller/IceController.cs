using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController: SpellController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedIce = Instantiate(spellData.Prefab);
        spawnedIce.transform.position = transform.position; //assign position to be same as this object, which is parented to player
        spawnedIce.GetComponent<IceBehavior>().DirectionChecker(castDir.MouseDirection()); // sets direction to that of the mouse
    }

}
