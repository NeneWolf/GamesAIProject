using UnityEngine;
using UnityEngine.AI;

internal class HealState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;

    bool hasBeenHealed;

    public HealState(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.Heal)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        hasBeenHealed = false;

        if(!enemyStateMachine.target.CompareTag("Heal"))
        {
            enemyStateMachine.FindTheNearestHealingTile();
        }

        if (enemyStateMachine.isThereHealing)
        {
            enemyStateMachine.FindPathToTarget(false);
            enemyStateMachine.UpdateAnimator("isPatrolling", true);
        }
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isPatrolling", false);
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        if(hasBeenHealed || !hasBeenHealed && !enemyStateMachine.isThereHealing)
        {
            enemyStateMachine.target = null;
            return EnemyStateMachine.EEnemyState.Patrol;
        }
        else if (enemyStateMachine.reportIsDead())
        {
            return EnemyStateMachine.EEnemyState.Dead;
        }
        else
            return stateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Heal"))
        {
            hasBeenHealed = true;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        Debug.Log("Heal State");
        if (!enemyStateMachine.target.CompareTag("Heal"))
        {
            enemyStateMachine.FindTheNearestHealingTile();
        }

        if(enemyStateMachine.target == null)
        {
            Debug.Log("Find the nearest healing tile");
            enemyStateMachine.FindTheNearestHealingTile();

            if (enemyStateMachine.isThereHealing)
            {
                enemyStateMachine.FindPathToTarget(false);
            }
            else return;
        }
    }
}