using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//base script of all melee behaviors

public class MeleeSpellBehavior : MonoBehaviour
{

    public float destroyAfterSeconds;

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    
}
