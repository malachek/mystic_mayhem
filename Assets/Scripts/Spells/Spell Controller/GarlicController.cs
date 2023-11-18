using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeController : SpellController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedBasicMelee = Instantiate(spellData.Prefab);
        spawnedBasicMelee.transform.position = transform.position;
        spawnedBasicMelee.transform.parent = transform;
    }
}
