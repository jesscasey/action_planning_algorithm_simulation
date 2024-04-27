using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CombatAgent : Agent
{
    Unit agentUnit;

    BattleSystem sys;

    public Text hint;

    void Start()
    {
        agentUnit = gameObject.GetComponent<Unit>();
    }

    // Initialise and reset agent
    public override void OnEpisodeBegin()
    {
        // Reset game if game ends
        if (sys.state == BattleSystem.BattleState.END)
        {
            sys.Start();
        }
    }

    // Observe environment
    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe agent and opponent's current health
        sensor.AddObservation(agentUnit.currentHealth);
        sensor.AddObservation(sys.rightUnit.currentHealth);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int decision = actions.DiscreteActions[0];

        if(decision == 0) { sys.Attack(); }
        if(decision == 1) { sys.Heal(); }
        if(decision == 2) { sys.Block(); }

        // If enemy is defeated
        if(sys.rightUnit.currentHealth <= 0)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // If agent is defeated
        if(agentUnit.currentHealth <= 0)
        {
            EndEpisode();
        }
    }
}
