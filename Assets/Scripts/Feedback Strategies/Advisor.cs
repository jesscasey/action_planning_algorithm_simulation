using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Advisor : MonoBehaviour
{
    [SerializeField]
    Unit player;

    [SerializeField]
    Unit enemy;

    [SerializeField]
    GameObject textBox;

    [SerializeField]
    Text advisorText;

    void Start()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        if (player.currentHealth < 20)
        {
            textBox.SetActive(true);
            advisorText.text = "You are low on health. Try healing or blocking.";
        }
        else if (enemy.isBlocking)
        {
            textBox.SetActive(true);
            advisorText.text = "The opponent is blocking. Attacking would be unwise.";
        }
        else if (enemy.currentHealth < 20)
        {
            textBox.SetActive(true);
            advisorText.text = "The opponent is low on health. Attack them!";
        }
        else if (player.currentHealth > 80)
        {
            textBox.SetActive(true);
            advisorText.text = "You are almost at full health. You should not heal at the moment.";
        }
        else
        {
            textBox.SetActive(false);
        }
    }
}
