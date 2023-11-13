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
            agent.SetDestination(enemyStateMachine.target.transform.position);
        else
        {
            enemyStateMachine.FieldOfViewCheckForTargets();
        }

        enemyStateMachine.UpdateAnimator("isChasing", true);
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isChasing", false);
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        // 0 - Melee 1 - Ranged
        var rang = Random.Range(0, 1);

        if (enemyStateMachine.CheckNeedsHealingStates())
        {
            return EnemyStateMachine.EEnemyState.Heal;
        }// TO CHANGE !
        else if (rang == 0 && agent.remainingDistance < 2.5f)
        {
            return EnemyStateMachine.EEnemyState.AttackMelee;
        }
        else if (rang == 1 && agent.remainingDistance < 6f)
        {
            return EnemyStateMachine.EEnemyState.AttackRanged;
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
            Debug.Log(enemyStateMachine.target);
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
        if(enemyStateMachine.target != null)
        {
            agent.SetDestination(enemyStateMachine.target.transform.position);
        }
        else return;
    }
}