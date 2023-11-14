using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int Hp = 10;
    // [SerializeField] int Strength = 1;

    public void TakeDamage(int damage){

        Hp -= damage;
        //Debug.Log("Player takes a hit.\n");
        //Debug.Log(Hp);
        if(Hp < 1){
            Debug.Log("Player is dead.");
        }
    }
}
