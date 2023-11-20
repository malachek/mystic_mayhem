using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellController : SpellController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedBasicSpell = Instantiate(prefab);
        spawnedBasicSpell.transform.position = transform.position; //assign position to be same as this object, which is parented to player
        spawnedBasicSpell.GetComponent<BasicSpellBehavior>().DirectionChecker(castDir.direction); // sets direction to that of the mouse
    }
}
