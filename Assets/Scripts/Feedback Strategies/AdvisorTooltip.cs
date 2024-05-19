using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvisorTooltip : MonoBehaviour
{
    [SerializeField]
    GameObject textBox;

    [SerializeField]
    Text message; // Message to be shown in the text box
    
    /* Indicates which feedback strategy is being used. If the personal advisor
     * strategy is being used, this variable is set to true. If the tooltip
     * strategy is being used, it is set to false. */
    [SerializeField]
    bool isAdvisor;

    void Start()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        if (BattleSystem.leftUnit.GetComponent<Unit>().currentHealth < 20)
        {
            textBox.SetActive(true);
            message.text = isAdvisor ? "You are low on health. Try healing or" +
                " blocking." : "Press 2 to heal";
            CombatAgent.suggestedActions = isAdvisor ? new Vector3(0, 1, 1) :
                new Vector3(0, 1, 0);
        }
        else if (BattleSystem.rightUnit.GetComponent<Unit>().isBlocking)
        {
            textBox.SetActive(true);
            message.text = isAdvisor ? "The opponent is blocking. Attacking" +
                " would be unwise." : "Press 3 to block";
            CombatAgent.suggestedActions = isAdvisor ? new Vector3(0, 1, 1) :
                new Vector3(0, 0, 1);
        }
        else if (BattleSystem.rightUnit.GetComponent<Unit>().currentHealth < 20)
        {
            textBox.SetActive(true);
            message.text = isAdvisor ? "The opponent is low on health. Attack" +
                " them!" : "Press 1 to attack";
            CombatAgent.suggestedActions = new Vector3(1, 0, 0);
        }
        else if (BattleSystem.leftUnit.GetComponent<Unit>().currentHealth > 80)
        {
            textBox.SetActive(true);
            message.text = isAdvisor ? "You are almost at full health. You" +
                " should not heal at the moment." : "Press 1 to attack";
            CombatAgent.suggestedActions = isAdvisor ? new Vector3(1, 0, 1) :
                new Vector3(1, 0, 0);
        }
        else
        {
            textBox.SetActive(false);
        }
    }
}
