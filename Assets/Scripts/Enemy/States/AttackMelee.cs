using UnityEngine;
using UnityEngine.AI;

internal class AttackMelee : BaseState<EnemyStateMachine.EEnemyState>
{
    EnemyStateMachine enemyStateMachine;
    NavMeshAgent agent;

    public AttackMelee(EnemyStateMachine enemyStateMachine, NavMeshAgent agent) : base(EnemyStateMachine.EEnemyState.AttackMelee)
    {
        this.enemyStateMachine = enemyStateMachine;
        this.agent = agent;
    }

    public override void EnterState()
    {
        enemyStateMachine.RotateToTarget();
        enemyStateMachine.UpdateAnimator("isAttacking", true);
    }

    public override void ExitState()
    {
        enemyStateMachine.UpdateAnimator("isAttacking", false);
    }

    public override EnemyStateMachine.EEnemyState GetNextState()
    {
        if (enemyStateMachine.CheckNeedsHealingStates() && enemyStateMachine.isThereHealing)
        {
            return EnemyStateMachine.EEnemyState.Heal;
        }
        //else if (enemyStateMachine.targetTile.CompareTag("Player") &&
        //    enemyStateMachine.targetTile.GetComponent<PlayerMovement>().ReportIsDead() ||
        //    (enemyStateMachine.targetTile.CompareTag("Village") || enemyStateMachine.targetTile.CompareTag("PlayerCastle")) && enemyStateMachine.targetTile.GetComponent<DetailMovement>().ReportStatus())
        //{
        //    return EnemyStateMachine.EEnemyState.Idle;
        //}
        else if (!enemyStateMachine.isPlayerInReachToAttack)
        {
            return EnemyStateMachine.EEnemyState.Chase;
        }
        else if (enemyStateMachine.reportIsDead())
        {
            return EnemyStateMachine.EEnemyState.Dead;
        }
        return stateKey;
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
        enemyStateMachine.FindPlayer();

        if(enemyStateMachine.isPlayerInReachToAttack)
            enemyStateMachine.RotateToTarget();
    }
}