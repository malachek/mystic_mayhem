using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehavior : ProjectileSpellBehavior
{
    float currentSlow;
    float slowDuration;

    protected override void Start()
    {
        base.Start();
        currentSlow = spellData.Slow;
        slowDuration = spellData.Slow;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == Vector3.zero)
        {
            Destroy(gameObject);
        }
        transform.position += direction * spellData.Speed * Time.deltaTime; //set movement of BasicSpell
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage(GetCurrentDamage()); //Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers
            enemy.SlowBy(currentSlow, slowDuration);
            reducePiere();
        }
        //Debug.Log("collided w/ "  + col.gameObject.name);
        //if (col.CompareTag("Maze"))
        //{
        //    Destroy(gameObject);
        //}
    }
}
