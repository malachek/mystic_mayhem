using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Player : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;
    public HealthBar healthBar;

    public int max_experience = 50;
    public int max_increase = 50;
    public int current_experience = 0;
    public ExperienceBar experienceBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(max_health);
        current_health = max_health;
        healthBar.SetHealth(current_health);

        experienceBar.SetMaxExperience(max_experience);
        current_experience = 0;
        experienceBar.SetExperience(current_experience);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GainExperience(15);
        }
    }

    void TakeDamage(int damage)
    {
        current_health -= damage;
        healthBar.SetHealth(current_health);
    }

    void GainExperience(int experience)
    {
        current_experience += experience;
        if (current_experience >= max_experience)
        {
            current_experience -= max_experience;
            max_experience += max_increase;
            experienceBar.SetMaxExperience(max_experience);
        }
        experienceBar.SetExperience(current_experience);
    }
}
