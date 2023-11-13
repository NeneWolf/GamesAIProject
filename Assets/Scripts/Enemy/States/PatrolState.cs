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
        // Constructor
    }

    public override void EnterState()
    {

        //TO BE CHANGED
        agent.SetDestination(RandomNavmeshLocation(enemyStateMachine.patrolRadius));

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
        if (agent.remainingDistance < 0.02f && !playerSpotted)
        {
            return EnemyStateMachine.EEnemyState.Idle;
        }
        else if (playerSpotted)
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

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += enemyStateMachine.enemy.transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}