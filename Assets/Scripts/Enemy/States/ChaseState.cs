using UnityEngine;
using UnityEngine.AI;

internal class ChaseState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;

    public ChaseState(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.Idle)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        Debug.Log("ChaseState");
        agent.SetDestination(enemyStateMachine.player.transform.position);
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }
}