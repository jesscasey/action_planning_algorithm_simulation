using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines the turn-based combat system used in the simulation.
/// </summary>
public class BattleSystem : MonoBehaviour
{
    // List of possible game states
    public enum BattleState
    {
        START,
        LEFTTURN,
        RIGHTTURN,
        END
    }

    public BattleState state;

    // Information about the unit on the left
    [SerializeField]
    GameObject leftUnitObject;
    [SerializeField]
    Transform leftUnitSpawn;
    [SerializeField]
    Unit leftUnit;
    
    // Information about the unit on the right
    [SerializeField]
    GameObject rightUnitObject;
    [SerializeField]
    Transform rightUnitSpawn;
    [SerializeField]
    Unit rightUnit;

    Unit currentUnit;
    Unit enemy;

    /// <summary>
    /// This method is called as soon as the scene is loaded.
    /// </summary>
    void Start()
    {
        state = BattleState.START;
        BattleSetup();
    }

    /// <summary>
    /// This method spawns the agents and initialises the game.
    /// </summary>
    void BattleSetup()
    {
        GameObject leftUnitInfo = Instantiate(leftUnitObject, leftUnitSpawn);
        leftUnit = leftUnitInfo.GetComponent<Unit>(); // Retrieve unit information

        GameObject rightUnitInfo = Instantiate(rightUnitObject, rightUnitSpawn);
        rightUnit = rightUnitInfo.GetComponent<Unit>();

        currentUnit = leftUnit;
        enemy = rightUnit;

        state = BattleState.LEFTTURN;
    }

    /// <summary>
    /// This method changes the game's state after a unit has performed an
    /// action. It also passes control to the next unit.
    /// </summary>
    void ChangeTurn()
    {
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : BattleState.LEFTTURN;
        currentUnit = state == BattleState.LEFTTURN ? leftUnit : rightUnit;
        enemy = state == BattleState.LEFTTURN ? rightUnit : leftUnit;

        // Units stop blocking on their next turn
        currentUnit.isBlocking = false;
    }

    /// <summary>
    /// When called by a unit, this method will reduce the other unit's health
    /// by a fixed amount.
    /// </summary>
    public void Attack()
    {
        // No damage will be done if the enemy is blocking
        bool isDead = enemy.isBlocking ? false : enemy.TakeDamage();

        if (isDead)
        {
            state = BattleState.END;
        }
        else
        {
            ChangeTurn();
        }
    }

    /// <summary>
    /// This method increases the current unit's health by a fixed amount.
    /// </summary>
    public void Heal()
    {
        currentUnit.Heal();
        ChangeTurn();
    }
}
