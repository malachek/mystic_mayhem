using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [Header("SpellStats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float maxCooldownDuration;
    float currentCooldown;
    public int pierce;

    protected CastDirection castDir;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        castDir = FindObjectOfType<CastDirection>();
        currentCooldown = maxCooldownDuration; //Resets cooldown
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
        currentCooldown = maxCooldownDuration;
    }
}
