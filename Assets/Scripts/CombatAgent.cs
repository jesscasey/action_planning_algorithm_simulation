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

    public Text hint;

    void Start()
    {
        agentUnit = gameObject.GetComponent<Unit>();
    }

    // Initialise and reset agent
    public override void OnEpisodeBegin()
    {

    }

    // Observe environment
    public override void CollectObservations(VectorSensor sensor)
    {
        // Throws error CS1503
        // sensor.AddObservation(hint.text);
    }
}
