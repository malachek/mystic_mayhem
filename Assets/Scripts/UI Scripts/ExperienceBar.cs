using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public CharacterStats characterStats;
    public UpgradeMenu upgradeMenu;
    private int playerLevel = 1;
    private bool levelUp = false;

    public Slider slider;

    void Update()
    {
        SetMaxExperience(characterStats.MaxXp);
        SetExperience(characterStats.Xp);
    }

    public void SetMaxExperience(int experience)
    {
        if (playerLevel != characterStats.Level)
        {
            levelUp = true;
        }
        slider.maxValue = experience;
    }

    public void SetExperience(int experience)
    {
        slider.value = experience;
        if (levelUp == true)
        {
            upgradeMenu.OpenUpgradeMenu();
            levelUp = false;
            playerLevel++;
        }
    }
}
