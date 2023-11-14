using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base script of all projectile behaviors to be placed on a prefab of a spell that is a projectile

public class ProjectileSpellBehavior : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    // Update is called once per frame

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        direction.Normalize();

        transform.up = direction; //sets direction of image to direction of movement

    }
}
