using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CharacterStats : MonoBehaviour
{
    public int Hp = 100;
    public int MaxHp = 100;
    public int Xp = 0;
    public int MaxXp = 100;
    public int Level = 1;
    // [SerializeField] int Strength = 1;

    [SerializeField] SpriteRenderer sprite; // to turn red when taking damage
    private float maxRedDuration = .2f;

    [SerializeField]
    GameObject startingSpell;
    [SerializeField]
    GameObject spell2;

    [SerializeField]
    GameObject spell3;

    public List<GameObject> spawnedSpells;

    InventoryManager inventory;
    public int spellIndex;
    
    void Start()
    {
        Hp = 100;
        MaxHp = 100;
        Xp = 0;
        MaxXp = 20;
    }

    private void Awake()
    {
        //Spawn the starting weapon

        inventory = GetComponent<InventoryManager>();

        SpawnSpell(startingSpell);
        SpawnSpell(spell2);
        SpawnSpell(spell3);

    }

    public void GainExperience(int experience)
    {
        Xp += experience;
        if (Xp >= MaxXp)
        {
            Xp -= MaxXp;
            MaxXp += 10;
            Level++;
        }
    }

    public void TakeDamage(int damage){
        StartCoroutine(ShowRedOnHit());
        Hp -= damage;
        Debug.Log("Player takes a hit.\n");
        //Debug.Log(Hp);
        //if(Hp > MaxHp)
        //{
        //    Hp = MaxHp;
        //}
        if(Hp < 1){
            Debug.Log("Player is dead.");
            Hp = 0;
        }
    }

    private IEnumerator ShowRedOnHit()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(maxRedDuration);
        sprite.color = Color.white;
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
