using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script of all projectile behaviors to be placed on a prefab of a spell that is a projectile

public class ProjectileSpellBehavior : MonoBehaviour
{
    public SpellSciptableObject spellData;
    protected Vector3 direction;
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


    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        direction.Normalize();

        transform.up = direction; //sets direction of image to direction of movement

    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("collided w/ "  + col.gameObject.name);
        if (col.CompareTag("Maze"))
        {
            Destroy(gameObject);
        }

        if(col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemy.TakeDamage((int)currentDamage); //Make sure to use currentDamage instead of weaponData.damage in case of any damage multipliers
            reducePiere();
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Maze"))
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void reducePiere()
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }

}
