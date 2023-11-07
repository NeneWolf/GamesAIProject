using UnityEngine;
using UnityEngine.InputSystem;

internal class IdleState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    bool playerSpotted = false;

    public IdleState(EnemyStateMachine enemy) : base(EnemyStateMachine.EEnemyState.Idle)
    {
        // Constructor

        enemyStateMachine = enemy;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
        enemyStateMachine.InitialFindAllTargetsInformation();

        enemyStateMachine.UpdateAnimator("isIdle", true);
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
 
        enemyStateMachine.UpdateAnimator("isIdle", false);
    }

    public override void UpdateState()
    {
        // Implement the behavior that should occur while in the Idle state
        Debug.Log("In Idle State");

        enemyStateMachine.FieldOfViewCheckForTargets();

        if (enemyStateMachine.canSeePlayer)
        {
            playerSpotted = true;
        }

    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        Debug.Log("Getting next state from Idle State");
        // Implement the logic to determine the next state (e.g., transition to Patrol or Chase based on certain conditions)
        if ((enemyStateMachine.currentNumberOfVillages > 0 || enemyStateMachine.isPlayerCastleAlive || enemyStateMachine.isPlayerAlive) && !playerSpotted)
        {
            return EnemyStateMachine.EEnemyState.Patrol;
        }
        else if (playerSpotted)
        {
            return EnemyStateMachine.EEnemyState.Chase;
        }
        else
        {
            return stateKey; // Stay in Idle state if no transition conditions are met
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        // Implement behavior for OnTriggerEnter in the Idle state
    }

    public override void OnTriggerStay(Collider other)
    {
        // Implement behavior for OnTriggerStay in the Idle state
    }

    public override void OnTriggerExit(Collider other)
    {
        // Implement behavior for OnTriggerExit in the Idle state
    }

}