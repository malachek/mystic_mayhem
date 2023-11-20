using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehavior : MeleeSpellBehavior
{
    List<GameObject> markedEnemies;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage((int)currentDamage);

            markedEnemies.Add(col.gameObject);//mark the enemy so it won't take another instance of damage from this garlic
        }
    }
}
