using UnityEngine;

internal class DeadState : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    bool waiting;

    public DeadState(EnemyStateMachine enemy, bool waiting) : base(EnemyStateMachine.EEnemyState.Dead)
    {
        enemyStateMachine = enemy;
        this.waiting = waiting;
    }

    public override void EnterState()
    {
        enemyStateMachine.UpdateAnimator("isDead", true);

        waiting = true; 

        if(!waiting)
        {
            enemyStateMachine.WaitForTime(10);
        }
        
    }

    public override void ExitState()
    {
        //throw new System.NotImplementedException();
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        return stateKey;
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
        if (!enemyStateMachine.waiting)
        {
            enemyStateMachine.KillEnemy();
        }
    }
}