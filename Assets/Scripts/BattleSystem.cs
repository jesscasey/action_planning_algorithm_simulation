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

        // Change colour of current unit to indicate whose turn it is
        currentUnit.gameObject.GetComponent<Renderer>().material.color =
            new Color(0, 136, 189);

        state = BattleState.LEFTTURN;
        Debug.Log("Left unit's turn");
    }

    void ChangeTurn()
    {
        Debug.Log("Changing turns...");
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : 
            BattleState.LEFTTURN;
        currentUnit = state == BattleState.LEFTTURN ? leftUnit : rightUnit;
        currentEnemy = state == BattleState.LEFTTURN ? rightUnit : leftUnit;

        // Units stop blocking on their next turn
        currentUnit.isBlocking = false;
        Debug.Log(state);

        currentUnit.gameObject.GetComponent<Renderer>().material.color = 
            new Color(0, 136, 189);
        currentEnemy.gameObject.GetComponent<Renderer>().material.color = 
            Color.white;
    }

    public void Attack()
    {
        // No damage will be done if the enemy is blocking
        bool isDead = currentEnemy.isBlocking ? false : 
            currentEnemy.TakeDamage();
        Debug.Log("Unit is attacking");

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
        Debug.Log("Unit is healing");
        ChangeTurn();
    }
}
