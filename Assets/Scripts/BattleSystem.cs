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

    public static GameObject leftUnit;
    public static GameObject rightUnit;

    public static Unit currentUnit;
    public static Unit currentEnemy;

    public void Start()
    {
        state = BattleState.START;
        Debug.Log("Initialising game...");

        /* Instantiate prefabs. This means that the unit variables will always use
         * newly instantiated prefabs, reducing the risk of a 
         * NullReferenceException. */
        try
        {
            // Ensures prefabs are only instantiated once
            if(!GameObject.FindWithTag("Player"))
            {
                leftUnit = Instantiate(leftUnitPrefab, new Vector3(-2.3f, 1f, -3f),
                    Quaternion.identity);
            }
            if(!GameObject.FindWithTag("Enemy"))
            {
                rightUnit = Instantiate(rightUnitPrefab, new Vector3(2.3f, 1f, -3f),
                    Quaternion.identity);
            }
        }
        catch (UnassignedReferenceException ex)
        {
            Debug.Log("Units not set in inspector");
        }

        Debug.LogFormat("Units instantiated: {0}, {1}", leftUnit.ToString(), 
            rightUnit.ToString());

        // Initialise units
        try
        {
            currentUnit = leftUnit.GetComponent<Unit>();
            currentEnemy = rightUnit.GetComponent<Unit>();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Units are null");
        }

        currentUnit.SetHealth();
        currentEnemy.SetHealth();

        Debug.Log("Current unit: " + currentUnit.ToString());
        Debug.Log("Current enemy: " + currentEnemy.ToString());
        Debug.Log("Left unit after initialisation: " + leftUnit.ToString());
        Debug.Log("Right unit after initialisation: " + rightUnit.ToString());

        // Change colour of current unit to indicate whose turn it is
        leftUnit.GetComponent<Renderer>().material.color =
            new Color(0, 136, 189);

        state = BattleState.LEFTTURN;
        Debug.Log("Left unit's turn");
    }

    void Update()
    {
        /* if(Input.GetKeyDown("1")) { Attack(); }
        if(Input.GetKeyDown("2")) { Heal(); }
        if(Input.GetKeyDown("3")) { Block(); } */
    }

    void ChangeTurn()
    {
        UnitNullCheck();
        Debug.Log("Changing turns...");
        state = state == BattleState.LEFTTURN ? BattleState.RIGHTTURN : 
            BattleState.LEFTTURN;
        Debug.Log(currentUnit.ToString() + currentEnemy.ToString());
        (currentUnit, currentEnemy) = (currentEnemy, currentUnit);
        Debug.Log(currentUnit.ToString() + currentEnemy.ToString());

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
        UnitNullCheck();
        // Debug.Log("Current enemy: " + currentEnemy.ToString());
        // Debug.Log(currentEnemy.isBlocking);

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
        UnitNullCheck();
        currentUnit.Heal();
        Debug.Log("Unit is healing");
        ChangeTurn();
    }

    public void Block()
    {
        UnitNullCheck();
        currentUnit.isBlocking = true;
        Debug.Log("Unit is blocking");
        ChangeTurn();
    }

    /* Checks whether units are null. Most of the errors occurring in this
     * script were caused by these variables becoming null at some point, so
     * this method helps to identify where and why they become null. */
    void UnitNullCheck()
    {
        if (!currentUnit) { Debug.Log("currentUnit is null"); }
        if (!currentEnemy) { Debug.Log("currentEnemy is null"); }
    }
}
