using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeController : SpellController
{

    GameObject spawnedBasicMelee;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        spawnedBasicMelee = Instantiate(spellData.Prefab);
    }

    protected override void Update()
    {
        base.Update();
        spawnedBasicMelee.transform.position = transform.position;
        spawnedBasicMelee.transform.parent = transform;
    }

    protected override void Attack()
    {
        base.Attack();
        GarlicBehavior.markedEnemies = new List<GameObject>();
    }
}
