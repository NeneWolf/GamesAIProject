using UnityEngine;
using UnityEngine.AI;

internal class ChaseState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;
    bool changeTarget;

    public ChaseState(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.Chase)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        if(enemyStateMachine.target != null)
            enemyStateMachine.FindPathToTarget();
        else
            enemyStateMachine.FieldOfViewCheckForTargets();

        enemyStateMachine.UpdateAnimator("isChasing", true);
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isChasing", false);
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        if (enemyStateMachine.CheckNeedsHealingStates() && enemyStateMachine.isThereHealing)
        {
            return EnemyStateMachine.EEnemyState.Heal;
        }
        else if (enemyStateMachine.hasReachedDestination && enemyStateMachine.movingToAttack && enemyStateMachine.target != null)
        {
            return EnemyStateMachine.EEnemyState.AttackMelee;
        }
        else if(changeTarget == false)
        {
            return EnemyStateMachine.EEnemyState.Idle;
        }
        else
        {
            return stateKey;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            changeTarget = true;
            enemyStateMachine.target = other.gameObject;
            enemyStateMachine.canSeeTarget = true;
        }
        else return;
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            changeTarget = false;
            enemyStateMachine.target = null;
            enemyStateMachine.canSeeTarget = false;
            agent.destination = enemyStateMachine.transform.position;
            enemyStateMachine.isBeingAttacked = false;
        }
        else return;
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            changeTarget = true;
            enemyStateMachine.target = other.gameObject;
        }
        else return;
    }

    public override void UpdateState()
    {
        if (enemyStateMachine.target == null)
        {
            enemyStateMachine.FieldOfViewCheckForTargets();
        }
    }
}