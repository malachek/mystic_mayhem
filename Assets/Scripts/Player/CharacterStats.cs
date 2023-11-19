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

        inventory = GetComponent<InventoryManager>();

        SpawnSpell(startingSpell);

    }

    public void TakeDamage(int damage){

        Hp -= damage;
        //Debug.Log("Player takes a hit.\n");
        //Debug.Log(Hp);
        if(Hp < 1){
            Debug.Log("Player is dead.");
        }
    }

    public void SpawnSpell(GameObject spell)
    {
        //checking if the slots are full, and returning if it is
        if(spellIndex >= inventory.spellSlots.Count -1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //Spawn the starting spell
        GameObject spawnedSpell = Instantiate(spell, transform.position, Quaternion.identity);
        spawnedSpell.transform.SetParent(transform);
        inventory.AddSpell(spellIndex, spawnedSpell.GetComponent<SpellController>());

        spellIndex++;
    }
}
