using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CombatAgent : Agent
{
    Unit agentUnit;

    [SerializeField]
    BattleSystem sys;

    public Text hint;

    void Start()
    {
        sys = GameObject.Find("Game Manager").GetComponent<BattleSystem>();
    }

    void Awake()
    {
        agentUnit = gameObject.GetComponent<Unit>();
        agentUnit.healthBar = GameObject.Find("RL Text")
            .GetComponent<UnityEngine.UI.Text>();
        // agentUnit.SetHealth();
    }

    // Initialise and reset agent
    public override void OnEpisodeBegin()
    {
        // Ensures BattleSystem's Start() method is only called once on startup
        if(sys.state != BattleSystem.BattleState.START) 
        { 
            sys.Start(); 
        }
    }

    // Observe environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentUnit.currentHealth);

        // Prevents returning NullReferenceException before enemy spawns
        if(GameObject.FindWithTag("Enemy"))
        {
            sensor.AddObservation(GameObject.FindWithTag("Enemy")
                .GetComponent<Unit>().currentHealth);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int decision = actions.DiscreteActions[0];

        switch(decision)
        {
            case 0:
                sys.Attack();
                break;
            case 1:
                sys.Heal();
                break;
            case 2:
                sys.Block();
                break;
        }

        // The action currently being suggested
        int suggestedAction = 0;

        if(hint)
        {
            if (hint.text == "Press 1 to attack") { suggestedAction = 0; }
            else if (hint.text == "Press 2 to heal") { suggestedAction = 1; }
            else if (hint.text == "Press 3 to block") { suggestedAction = 2; }
        }

        /* Provide positive reward if agent performs action suggested by the
         * hint, and a negative reward otherwise */
        if (suggestedAction == decision) { AddReward(1.0f); }
        else { AddReward(-1.0f); }

        // If enemy is defeated
        if(sys.rightUnit.GetComponent<Unit>().currentHealth <= 0)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // If agent is defeated
        if(agentUnit.currentHealth <= 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    // Allows agent to be controlled by a human for testing purposes
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var disActionsOut = actionsOut.DiscreteActions;

        // Listen for keyboard input
        KeyDownEvent e = new KeyDownEvent();
        disActionsOut[0] = (int)e.character;
    }
}
