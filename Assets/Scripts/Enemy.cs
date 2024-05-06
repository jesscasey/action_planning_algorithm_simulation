using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    BattleSystem battle;

    Unit unitInfo;

    void Awake()
    {
        unitInfo = gameObject.GetComponent<Unit>();
        unitInfo.healthBar = GameObject.Find("Enemy").GetComponent<UnityEngine.UI.Text>();
        // unitInfo.SetHealth();
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
            else if (battle.leftUnit.GetComponent<Unit>().isBlocking)
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
