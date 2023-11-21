using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.U2D;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] public CharacterStatsScriptableObject stats;
    [SerializeField] SpriteRenderer sprite; // to turn red when taking damage

    GameObject currentStartingSpell;

    [HideInInspector] public float currentPower;
    [HideInInspector] public float currentArmor;
    [HideInInspector] public int currentMaxHealth;
    [HideInInspector] public int currentHealth;
    [HideInInspector] public int currentHealthRegen;
    [HideInInspector] public float currentMaxDashCooldown;
    [HideInInspector] public float currentProjectileSpeed;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public int currentProjectileAmount;
    [HideInInspector] public float currentExperienceModifier;

    [Header("Need to make these private with getter setters")]
    public int Xp;
    public int MaxXp;
    public int Level = 1;

    [Header("For Testing Purposes Only")]
    [SerializeField]
    GameObject spell2;
    [SerializeField]
    GameObject spell3;
    [SerializeField]
    GameObject spell4;

    public List<GameObject> spawnedSpells;

    InventoryManager inventory;
    public int spellIndex;
    
    
    void Awake()
    {
         currentStartingSpell = stats.StartingSpell;
         currentPower = stats.Power;
         currentArmor = stats.Armor;
         currentMaxHealth = stats.MaxHealth;
         currentHealth = stats.MaxHealth;
         currentHealthRegen = stats.HealthRegen;
         currentMaxDashCooldown = stats.MaxDashCooldown;
         currentProjectileSpeed = stats.ProjectileSpeed;
         currentMoveSpeed = stats.MoveSpeed;
         currentProjectileAmount = stats.ProjectileAmount;
         currentExperienceModifier = stats.ExperienceModifier;

        inventory = GetComponent<InventoryManager>();

        SpawnSpell(currentStartingSpell);
        SpawnSpell(spell2);
        SpawnSpell(spell3);
        SpawnSpell(spell4);
    }
    void Start()
    {
        Xp = 0;
        MaxXp = 20;
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
        currentHealth -= damage;
        Debug.Log("Player takes a hit.\n");
        //Debug.Log(currentHealth);
        //if(currentHealth > stats.MaxHealth)
        //{
        //    currentHealth = MaxHealth;
        //}
        if (currentHealth < 1){
            Debug.Log("Player is dead.");
            currentHealth = 0;
        }
    }

    private IEnumerator ShowRedOnHit()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.2f);
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
