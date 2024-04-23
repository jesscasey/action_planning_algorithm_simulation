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
        // Throws error CS1503
        // sensor.AddObservation(hint.text);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Define actions here

        // If enemy is defeated
        if(sys.rightUnit.currentHealth <= 0)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // If agent is defeated
        if(sys.leftUnit.currentHealth <= 0)
        {
            EndEpisode();
        }
    }
}
