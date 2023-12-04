using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffBehavior : ProjectileSpellBehavior
{

    protected override void Start()
    {
        base.Start();
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

    

}
