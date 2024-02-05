using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class stores and updates information about a unit at run-time.
/// </summary>
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

    /// <summary>
    /// This method initialises the unit's current number of health points and
    /// its corresponding health bar.
    /// </summary>
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.text = UpdateHealthBar();
    }

    /// <summary>
    /// This method updates the unit's health bar to reflect the unit's current
    /// number of health points.
    /// </summary>
    /// <returns>
    /// A string consisting of the unit's name, its current number of health
    /// points, and its maximum possible number of health points.
    /// </returns>
    string UpdateHealthBar()
    {
        return System.String.Format("{0}: {1}/{2}", gameObject.name, currentHealth, maxHealth);
    }

    /// <summary>
    /// This method reduces the unit's health points by a given amount.
    /// </summary>
    /// <returns>
    /// True if the unit's health is equal to 0, otherwise false.
    /// </returns>
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

    /// <summary>
    /// This method increases the unit's health points by a given amount.
    /// </summary>
    public void Heal()
    {
        // Ensure unit's health does not exceed maximum number of health points
        currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth + healAmount;
        healthBar.text = UpdateHealthBar();
    }

    /// <summary>
    /// This method blocks damage during the next turn.
    /// </summary>
    public void Block()
    {
        isBlocking = true;
    }
}
