using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int Hp = 10;
    // [SerializeField] int Strength = 1;

    [SerializeField]
    GameObject startingSpell;

    public List<GameObject> spawnedSpells;

    InventoryManager inventory;
    public int spellIndex;

    private void Awake()
    {
        //Spawn the starting weapon
        SpawnSpell(startingSpell);

        inventory = GetComponent<InventoryManager>();
    }

    public void TakeDamage(int damage){

        Hp -= damage;
        Debug.Log("Player takes a hit.\n");
        Debug.Log(Hp);
        if(Hp < 1){
            Debug.Log("Player is dead.");
        }
    }

    public void SpawnSpell(GameObject spell)
    {
        GameObject spawnedSpell = Instantiate(spell, transform.position, Quaternion.identity);
        spawnedSpell.transform.SetParent(transform);
        spawnedSpells.Add(spawnedSpell);
    }
}
