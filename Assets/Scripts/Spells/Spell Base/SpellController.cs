using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [Header("SpellStats")]
    public SpellSciptableObject spellData;
    float currentCooldown;

    protected CastDirection castDir;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        castDir = FindObjectOfType<CastDirection>();
        currentCooldown = spellData.MaxCooldownDuration; //Resets cooldown
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime; 
        if(currentCooldown <= 0f) 
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = spellData.MaxCooldownDuration;
    }
}
