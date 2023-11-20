using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellBehavior : ProjectileSpellBehavior
{

    BasicSpellController bsc;

    protected override void Start()
    {
        base.Start();
        bsc = FindObjectOfType<BasicSpellController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * bsc.speed * Time.deltaTime; //set movement of BasicSpell
    }
}
