using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    BattleSystem battle;

    Unit unitInfo;

    void Start()
    {
        unitInfo = gameObject.GetComponent<Unit>();
    }

    void Update()
    {
        // Prevent enemy from acting out of turn
        if (battle.state == BattleSystem.BattleState.RIGHTTURN)
        {
            if (unitInfo.currentHealth < 30)
            {
                battle.Heal();
            }
            else if (battle.leftUnit.isBlocking)
            {
                battle.Block();
            }
            else
            {
                battle.Attack();
            }
        }
    }
}
