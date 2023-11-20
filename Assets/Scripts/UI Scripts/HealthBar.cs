using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public CharacterStats characterStats;
    public DeathMenu deathMenu;

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    void Update()
    {
        SetMaxHealth(characterStats.MaxHp);
        SetHealth(characterStats.Hp);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (health < 1)
        {
            deathMenu.OpenDeathMenu();
        }
    }
}
