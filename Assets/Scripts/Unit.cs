using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    // For fairness, values should be the same across all units
    int maxHealth = 100;
    int healAmount = 10; // Health points gained when unit heals itself
    int damage = 10; // Health points enemy loses when unit attacks

    int currentHealth;

    [SerializeField]
    Text healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.text = UpdateHealthBar();
    }

    /* Display unit's current number of health points compared to the
     * maximum possible number */
    string UpdateHealthBar()
    {
        return String.Format("{0}/{1}", currentHealth, maxHealth);
    }

    public bool TakeDamage()
    {
        currentHealth -= damage;

        // Ensure unit's health is not less than 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // If the method returns true, the game ends as the unit has died
        if (currentHealth == 0)
        {
            return true;
        }

        return false;
    }

    public void Heal()
    {
        // Ensure unit's health does not exceed maximum number of health points
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth + healAmount;
        healthBar.text = UpdateHealthBar();
    }
}
