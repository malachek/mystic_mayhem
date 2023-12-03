using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.U2D;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] public CharacterStatsScriptableObject stats;
    [SerializeField] SpriteRenderer sprite; // to turn red when taking damage

    [SerializeField] public GameObject spawner;
    GameObject currentStartingSpell;

    [HideInInspector] public float currentPower;
    [HideInInspector] public float currentArmor;
    [HideInInspector] public int currentMaxHealth;
    [HideInInspector] public int currentHealthRegen;
    [HideInInspector] public float currentMaxDashCooldown;
    [HideInInspector] public float currentProjectileSpeed;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public int currentProjectileAmount;
    [HideInInspector] public float currentExperienceModifier;

    public int currentHealth;

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
    [SerializeField]
    GameObject passiveItem1;
    [SerializeField]
    GameObject passiveItem2;

    //public List<GameObject> spawnedSpells;

    InventoryManager inventory;
    public int spellIndex;
    public int passiveItemIndex;

    private bool trig;
    
    
    void Awake()
    {
        trig = false;
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


        //TESTING
        SpawnSpell(spell2);
        SpawnSpell(spell3);
        SpawnSpell(spell4);

        SpawnPassiveItem(passiveItem1);
        SpawnPassiveItem(passiveItem2);
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
            
            if(Level > 2 & !(trig))
            {
                trig = true;
                spawner.GetComponent<Monster1Manager>().start_spawning_2 = true;
            }
        }
    }

    public void TakeDamage(int damage){
        StartCoroutine(ShowRedOnHit());
        currentHealth -= (int)(damage / currentArmor);
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

        GameObject spawnedSpell = Instantiate(spell, transform.position, Quaternion.identity);
        spawnedSpell.transform.SetParent(transform);
        inventory.AddSpell(spellIndex, spawnedSpell.GetComponent<SpellController>());

        spellIndex++;
    }
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        //checking if the slots are full, and returning if it is
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}
