using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFunctionality : MonoBehaviour
{
    [SerializeField] int hp = 10;
    [SerializeField] int strength = 1;

    float lastAttack;
    [SerializeField] float cooldown = 1; //bigger cooldown, slower attacks

    public int EXP = 1;

    public CharacterStats player; //declare whatever the permanent character class is here later
    //permanent character class must have method TakeDamage() for attack to work

    public KillCount killCounter;

    //attack will happen every second of contact with the player
    //can change rate of attack by changing 'cooldown' var
    private void OnCollisionStay2D(Collision2D collision){
        if (Time.time - lastAttack < cooldown) return;

        if(collision.gameObject.name == "Player"){
            Attack();
            
            lastAttack = Time.time;

        }
    }

    private void Attack(){
        player.TakeDamage(strength);
    }

    //enemy can take damage here. Object destroyed upon reaching 0 health, implement later
    public void TakeDamage(int damage){
        hp -= damage;
        Debug.Log("Monster takes a hit.\n");
        // Debug.Log(hp);
        if(hp < 1){
            Debug.Log(gameObject);
            DropEXP();
            Destroy(gameObject);

        }
    }

    public void DropEXP(){
        killCounter.kills_amt += EXP;
    }


}