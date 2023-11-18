using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base script of all melee behaviors

public class MeleeSpellBehavior : MonoBehaviour
{
    public SpellSciptableObject spellData;

    public float destroyAfterSeconds;

    //Current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentMaxCooldownDuration;
    protected int currentPierce;

    private void Awake()
    {
        currentDamage = spellData.Damage;
        currentSpeed = spellData.Speed;
        currentMaxCooldownDuration = spellData.MaxCooldownDuration;
        currentPierce = spellData.Pierce;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyFunctionality enemy = col.GetComponent<EnemyFunctionality>();
            enemy.TakeDamage((int)currentDamage); //Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers
        }

    }

}
