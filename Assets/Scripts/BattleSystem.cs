using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    Unit leftUnit;
    
    [SerializeField]
    Unit rightUnit;

    Unit currentUnit;
    Unit currentEnemy;

    void Start()
    {
        state = BattleState.START;
        BattleSetup();
    }

    // Initialise game
    void BattleSetup()
    {
        currentUnit = leftUnit;
        currentEnemy = rightUnit;

        state = BattleState.LEFTTURN;
    }

    void ChangeTurn()
    {
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : BattleState.LEFTTURN;
        currentUnit = state == BattleState.LEFTTURN ? leftUnit : rightUnit;
        currentEnemy = state == BattleState.LEFTTURN ? rightUnit : leftUnit;

        // Units stop blocking on their next turn
        currentUnit.isBlocking = false;
    }

    public void Attack()
    {
        // No damage will be done if the enemy is blocking
        bool isDead = currentEnemy.isBlocking ? false : currentEnemy.TakeDamage();

        if (isDead)
        {
            state = BattleState.END;
        }
        else
        {
            ChangeTurn();
        }
    }

    public void Heal()
    {
        currentUnit.Heal();
        ChangeTurn();
    }
}
