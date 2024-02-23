using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    // For fairness, values should be the same across all units
    int maxHealth = 100;
    int healAmount = 10; // Health points gained when unit heals itself
    int damage = 10; // Health points lost when unit is attacked

    int currentHealth;

    [SerializeField]
    Text healthBar;

    public bool isBlocking;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.text = UpdateHealthBar();
    }

    string UpdateHealthBar()
    {
        return System.String.Format("{0}: {1}/{2}", gameObject.name, currentHealth, maxHealth);
    }

    // Reduce unit's health by a given amount
    public bool TakeDamage()
    {
        currentHealth -= damage;

        // Ensure unit's health is not less than 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.text = UpdateHealthBar();

        // If the method returns true, the game ends as the unit has died
        if (currentHealth == 0)
        {
            return true;
        }

        return false;
    }

    // Increase unit's health by a given amount
    public void Heal()
    {
        // Ensure unit's health does not exceed maximum number of health points
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth + healAmount;
        healthBar.text = UpdateHealthBar();
    }

    // Block damage during next turn
    public void Block()
    {
        isBlocking = true;
    }
}
