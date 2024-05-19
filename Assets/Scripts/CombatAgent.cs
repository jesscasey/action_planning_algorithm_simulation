using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Sensors.Reflection;
using Unity.MLAgents.Actuators;

public class CombatAgent : Agent
{
    Unit agentUnit;

    [SerializeField]
    BattleSystem sys;

    /* Indicates which actions the agent should take based on the hint
     * currently being displayed. The axes represent the available actions;
     * x = attack, y = heal, and z = block. If a hint recommends a specific
     * action, the axis representing that action will be set to 1, with the
     * remaining axes being set to 0. The opposite is true if a hint
     * discourages a specific action. */
    [Observable]
    public static Vector3 suggestedActions;

    void Start()
    {
        sys = GameObject.Find("Game Manager").GetComponent<BattleSystem>();
        suggestedActions = Vector3.zero;
    }

    void Awake()
    {
        agentUnit = gameObject.GetComponent<Unit>();
        agentUnit.healthBar = GameObject.Find("RL Text")
            .GetComponent<UnityEngine.UI.Text>();
    }

    // Initialise and reset agent
    public override void OnEpisodeBegin()
    {
        // Ensures BattleSystem's Start() method is only called once on startup
        if(sys.state != BattleSystem.BattleState.START) 
        { 
            sys.Start(); 
        }

        suggestedActions = Vector3.zero;
    }

    // Observe environment
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentUnit.currentHealth);

        // Prevents returning NullReferenceException before enemy spawns
        if(BattleSystem.rightUnit)
        {
            sensor.AddObservation(BattleSystem.rightUnit.GetComponent<Unit>()
                .currentHealth);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        /* The desired action at a given step. This is usually an action that
         * is recommended by the hint currently being displayed. */
        int desiredAction = 0;

        /* If no actions are suggested, i.e. if no hints are currently being
         * provided */
        if(suggestedActions == Vector3.zero)
        {
            if (agentUnit.currentHealth < 20) { desiredAction = 1; }
            else if (BattleSystem.rightUnit.GetComponent<Unit>().isBlocking)
            {
                desiredAction = 2;
            }
            else { desiredAction = 0; } // Agent attacks by default
        }
        else
        {
            if(suggestedActions.x == 1) { desiredAction = 0; }
            else if(suggestedActions.y == 1) { desiredAction = 1; }
            /* If no other axes are equal to 1, and the vector is NOT equal to
             * Vector3.zero, then z must be 1, thus the hint must be suggesting
             * to block */
            else { desiredAction = 2; }
        }

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

        /* Provide positive reward if agent performs action suggested by the
         * hint, and a negative reward otherwise */
        if(desiredAction == decision) { AddReward(1.0f); }
        else { AddReward(-1.0f); }

        // If enemy is defeated
        if(BattleSystem.rightUnit.GetComponent<Unit>().currentHealth <= 0)
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
        Debug.Log("Key pressed: " + disActionsOut[0]);
    }
}
