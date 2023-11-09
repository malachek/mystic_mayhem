using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int Hp = 10;
    // [SerializeField] int Strength = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage){
        if (Hp > 0){
            Hp -= damage;
            Debug.Log("Player takes a hit.\n");
            Debug.Log(Hp);
        }else{
            Hp = 0;
            Debug.Log("Player is dead.");
        }
    }
}
