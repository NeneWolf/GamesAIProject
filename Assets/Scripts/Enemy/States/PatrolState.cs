using UnityEngine;
using UnityEngine.AI;

internal class PatrolState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;
    bool playerSpotted;

    public PatrolState(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.Patrol)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        enemyStateMachine.FindPathToRandomPoint();

        enemyStateMachine.UpdateAnimator("isPatrolling",true);

        enemyStateMachine.FieldOfViewCheckForTargets();

        if (enemyStateMachine.canSeeTarget)
        {
            playerSpotted = true;
        }
        else { playerSpotted = false; }
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isPatrolling", false);
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        if (enemyStateMachine.CheckNeedsHealingStates() && enemyStateMachine.isThereHealing)
        {
            return EnemyStateMachine.EEnemyState.Heal;
        }
        else if (enemyStateMachine.hasReachedDestination && !playerSpotted)
        {
            return EnemyStateMachine.EEnemyState.Idle;
        }
        else if (playerSpotted || enemyStateMachine.isBeingAttacked)
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
        
    }

    public override void OnTriggerExit(Collider other)
    {
       
    }

    public override void OnTriggerStay(Collider other)
    {
        
    }

    public override void UpdateState()
    {
        enemyStateMachine.FieldOfViewCheckForTargets();

        if (enemyStateMachine.canSeeTarget)
        {
            playerSpotted = true;
        }
        else { playerSpotted = false;}

    }
}