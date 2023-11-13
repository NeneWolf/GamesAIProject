using UnityEngine;
using UnityEngine.AI;

internal class AttackRanged : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;

    public AttackRanged(EnemyStateMachine enemyStateMachine) : base(EnemyStateMachine.EEnemyState.AttackRanged)
    {
        this.enemyStateMachine = enemyStateMachine;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
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