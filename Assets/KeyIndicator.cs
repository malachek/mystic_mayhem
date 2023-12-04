using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based off of https://www.youtube.com/watch?v=U1SdjGUFSAI...
 */

public class KeyIndicator : MonoBehaviour
{
    public Transform key;

    // Update is called once per frame
    void Update()
    {
        var direction = key.position - transform.position;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
