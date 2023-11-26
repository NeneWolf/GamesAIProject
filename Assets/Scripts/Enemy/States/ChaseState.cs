using UnityEngine;
using UnityEngine.AI;

internal class ChaseState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;
    bool targetLeftAreaOfChase;

    public ChaseState(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.Chase)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        targetLeftAreaOfChase = false;

        enemyStateMachine.FindPlayer();

        if (enemyStateMachine.target != null && !enemyStateMachine.isPlayerInReachToAttack)
            enemyStateMachine.FindPathToTarget(true);
        else if(!enemyStateMachine.isBeingCalledForHelp)
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
        else if (enemyStateMachine.isPlayerInReachToAttack)
        {
            return EnemyStateMachine.EEnemyState.AttackMelee;
        }
        else if (targetLeftAreaOfChase)
        {
            return EnemyStateMachine.EEnemyState.Idle;
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
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    targetLeftAreaOfChase = true;
        //    enemyStateMachine.targetTile = other.gameObject;
        //    enemyStateMachine.canSeeTarget = true;
        //}
        //else return;
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            targetLeftAreaOfChase = true;
            enemyStateMachine.target = null;
            enemyStateMachine.canSeeTarget = false;
        }
        else return;
    }

    public override void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.CompareTag("Player"))
        //{
        //    targetLeftAreaOfChase = true;
        //    enemyStateMachine.targetTile = other.gameObject;
        //}
        //else return;
    }

    public override void UpdateState()
    {
        if(enemyStateMachine.target != null)
        {
           if(!enemyStateMachine.isBeingCalledForHelp)
                enemyStateMachine.FieldOfViewCheckForTargets();

            enemyStateMachine.FindPlayer();

            if (!enemyStateMachine.isPlayerInReachToAttack && enemyStateMachine.hasReachedDestination)
            {
                enemyStateMachine.FindPathToTarget(true);
            }
        }
    }
}