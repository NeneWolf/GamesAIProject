using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

internal class IdleState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    bool targetSpotted = false;
    bool waiting;

    public IdleState(EnemyStateMachine enemy, bool waiting) : base(EnemyStateMachine.EEnemyState.Idle)
    {
        // Constructor

        enemyStateMachine = enemy;
        this.waiting = waiting;
    }

    public override void EnterState()
    {
        enemyStateMachine.InitialFindAllTargetsInformation();
        enemyStateMachine.UpdateAnimator("isIdle", true);

        if (enemyStateMachine.canSeeTarget)
        {
            targetSpotted = true;
        }
        else { targetSpotted = false; waiting = true; enemyStateMachine.WaitForTime(10); }
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isIdle", false);
    }

    public override void UpdateState()
    {
        enemyStateMachine.FieldOfViewCheckForTargets();

        if (enemyStateMachine.canSeeTarget)
        {
            targetSpotted = true;
        }
        else { targetSpotted = false; waiting = enemyStateMachine.waiting; }
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        if ((enemyStateMachine.currentNumberOfVillages > 0 || enemyStateMachine.isPlayerCastleAlive || enemyStateMachine.isPlayerAlive) && !targetSpotted && waiting == false)
        {
            return EnemyStateMachine.EEnemyState.Patrol;
        }
        else if (targetSpotted || enemyStateMachine.isBeingAttacked)
        {
            return EnemyStateMachine.EEnemyState.Chase;
        }
        else
        {
            return stateKey; 
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