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

    // Information about the unit on the left
    [SerializeField]
    GameObject leftUnitObject;
    [SerializeField]
    Unit leftUnit;
    
    // Information about the unit on the right
    [SerializeField]
    GameObject rightUnitObject;
    [SerializeField]
    Unit rightUnit;

    Unit currentUnit;
    Unit enemy;

    void Start()
    {
        state = BattleState.START;
        BattleSetup();
    }

    // Initialise game
    void BattleSetup()
    {
        leftUnit = leftUnitObject.GetComponent<Unit>(); // Retrieve unit information
        rightUnit = rightUnitObject.GetComponent<Unit>();

        currentUnit = leftUnit;
        enemy = rightUnit;

        state = BattleState.LEFTTURN;
    }

    void ChangeTurn()
    {
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : BattleState.LEFTTURN;
        currentUnit = state == BattleState.LEFTTURN ? leftUnit : rightUnit;
        enemy = state == BattleState.LEFTTURN ? rightUnit : leftUnit;

        // Units stop blocking on their next turn
        currentUnit.isBlocking = false;
    }

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

    public void Heal()
    {
        currentUnit.Heal();
        ChangeTurn();
    }
}
