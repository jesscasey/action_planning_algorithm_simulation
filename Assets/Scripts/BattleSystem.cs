using System;
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

    public GameObject leftUnit;
    public GameObject rightUnit;

    // Temp variables for instantiating
    GameObject leftUnitInstantiate;
    GameObject rightUnitInstantiate;

    Unit currentUnit;
    Unit currentEnemy;

    void Start()
    {
        state = BattleState.START;
        BattleSetup();
        Debug.Log("Initialising game...");
    }

    // Initialise game
    public void BattleSetup()
    {
        try
        {
            leftUnitInstantiate = Instantiate(leftUnit, new Vector3(-2.3f, 0f, 0f), Quaternion.identity);
            rightUnitInstantiate = Instantiate(rightUnit, new Vector3(2.3f, 0f, 0f), Quaternion.identity);
        }
        catch(NullReferenceException ex)
        {
            Debug.Log("Units not set in inspector");
        }
    }

    void Update()
    {
        if(state == BattleState.START)
        {
            try
            {
                currentUnit = leftUnitInstantiate.GetComponent<Unit>();
                currentEnemy = rightUnitInstantiate.GetComponent<Unit>();
                currentUnit.SetHealth();
                currentEnemy.SetHealth();
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Units are null");
            }

            // Change colour of current unit to indicate whose turn it is
            leftUnitInstantiate.GetComponent<Renderer>().material.color =
                new Color(0, 136, 189);

            state = BattleState.LEFTTURN;
            Debug.Log("Left unit's turn");
        }
    }

    void ChangeTurn()
    {
        Debug.Log("Changing turns...");
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : 
            BattleState.LEFTTURN;
        currentUnit = state == BattleState.LEFTTURN ? leftUnitInstantiate.GetComponent<Unit>() : rightUnitInstantiate.GetComponent<Unit>();
        currentEnemy = state == BattleState.LEFTTURN ? rightUnitInstantiate.GetComponent<Unit>() : leftUnitInstantiate.GetComponent<Unit>();

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

    public void Block()
    {
        currentUnit.isBlocking = true;
        Debug.Log("Unit is blocking");
        ChangeTurn();
    }
}
