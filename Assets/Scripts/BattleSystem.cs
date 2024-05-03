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

    // Temp variables for instantiating prefabs
    [SerializeField] GameObject leftUnitPrefab;
    [SerializeField] GameObject rightUnitPrefab;

    public GameObject leftUnit;
    public GameObject rightUnit;

    Unit currentUnit;
    Unit currentEnemy;

    void Start()
    {
        state = BattleState.START;
        BattleSetup();
        Debug.Log("Initialising game...");
    }

    /* Instantiate prefabs. This means that the unit variables will always use
     * newly instantiated prefabs, reducing the risk of a 
     * NullReferenceException. */
    public void BattleSetup()
    {
        try
        {
            leftUnit = Instantiate(leftUnitPrefab, new Vector3(-2.3f, 1f, -3f), Quaternion.identity);
            rightUnit = Instantiate(rightUnitPrefab, new Vector3(2.3f, 1f, -3f), Quaternion.identity);
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
                currentUnit = leftUnit.GetComponent<Unit>();
                currentEnemy = rightUnit.GetComponent<Unit>();
                currentUnit.SetHealth();
                currentEnemy.SetHealth();
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Units are null");
            }

            // Change colour of current unit to indicate whose turn it is
            leftUnit.GetComponent<Renderer>().material.color =
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
        currentUnit = state == BattleState.LEFTTURN ? leftUnit.GetComponent<Unit>() : rightUnit.GetComponent<Unit>();
        currentEnemy = state == BattleState.LEFTTURN ? rightUnit.GetComponent<Unit>() : leftUnit.GetComponent<Unit>();

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
