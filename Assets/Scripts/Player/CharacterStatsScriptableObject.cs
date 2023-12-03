using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterStatsScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject startingSpell;
    public GameObject StartingSpell { get => startingSpell; private set => startingSpell = value; }

    [SerializeField]
    float power;
    public float Power { get => power; private set => power = value; }

    [SerializeField]
    float armor;
    public float Armor { get => armor; private set => armor = value; }

    [SerializeField]
    int maxHealth;
    public int MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    int healthRegen;
    public int HealthRegen { get => healthRegen; private set => healthRegen = value; }

    [SerializeField]
    float maxDashCooldown;
    public float MaxDashCooldown { get => maxDashCooldown; private set => maxDashCooldown = value; }

    [SerializeField]
    float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    int projectileAmount;
    public int ProjectileAmount { get => projectileAmount; private set => projectileAmount = value; }

    [SerializeField]
    float experienceModifier;
    public float ExperienceModifier { get => experienceModifier; private set => experienceModifier = value; }

}
