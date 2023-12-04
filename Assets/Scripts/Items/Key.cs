using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject theyAreComing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            boss.SetActive(true); // waoh...
            theyAreComing.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
