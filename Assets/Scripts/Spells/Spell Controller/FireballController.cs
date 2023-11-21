using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : SpellController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedFireball = Instantiate(spellData.Prefab);
        spawnedFireball.transform.position = transform.position; //assign position to be same as this object, which is parented to player
        spawnedFireball.GetComponent<FireballBehavior>().DirectionChecker(castDir.ClosestEnemyDirection()); // sets direction to that of the mouse
    }

}
