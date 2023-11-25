using UnityEngine;
using UnityEngine.AI;

internal class PatrolState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;

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
        else if (enemyStateMachine.hasReachedDestination && !enemyStateMachine.canSeeTarget)
        {
            return EnemyStateMachine.EEnemyState.Idle;
        }
        else if (enemyStateMachine.canSeeTarget)
        {
            return EnemyStateMachine.EEnemyState.Chase;
        }
        else if (enemyStateMachine.reportIsDead())
        {
            return EnemyStateMachine.EEnemyState.Dead;
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
    }
}