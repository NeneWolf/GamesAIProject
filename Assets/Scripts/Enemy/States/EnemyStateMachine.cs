using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EEnemyState>
{
    NavMeshAgent navMeshAgent;
    Animator animator;

    public GameObject enemy;

    // States of the enemy
    [Header("EnemyStats")]
    int currentHealth;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int damage = 10;

    [SerializeField] float attackRange = 2f;
    [SerializeField] float chaseRange = 5f;

    //Tracking information
    public int currentNumberOfVillages;
    GameObject[] villages;
    GameObject closesVillage;

    public bool isPlayerCastleAlive = true;
    GameObject playerCastle;

    public bool isPlayerAlive = true;
    public GameObject player;

    public int patrolRadius = 10;   

    [Header("FieldOfView")]
    [Space(10)]
    public bool canSeePlayer = false;
    public LayerMask whatIsPlayerMask;

    [Space(10)]
    public float radius;
    [Range(0, 360)]
    public float angle;

    EEnemyState nextStateKey;

    // Enum of the enemy states
    public enum EEnemyState
    {
        Idle,
        Patrol,
        Heal,
        Chase,
        Attack,
        Dead
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemy = this.gameObject;

        AddStates();

        // Initialize the enemy's health
        currentHealth = maxHealth;
    }

    public void InitialFindAllTargetsInformation()
    {
        // Initialize the number of villages
        currentNumberOfVillages = GameObject.FindGameObjectsWithTag("Village").Length;

        // Initialize the villages array
        villages = GameObject.FindGameObjectsWithTag("Village");

        // Initialize the player castle
        playerCastle = GameObject.FindGameObjectWithTag("PlayerCastle");
    }


    public void Update()
    {
        nextStateKey = currentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(currentState.stateKey))
        {
            currentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            TransitionState(nextStateKey);
        }


        if (currentHealth <= 0)
        {
            TransitionState(EEnemyState.Dead);
        }

        FieldOfViewCheckForTargets();
    }

    void AddStates()
    {
        // Initialize your enemy states and add them to the 'states' dictionary
        states.Add(EEnemyState.Idle, new IdleState(this));
        states.Add(EEnemyState.Patrol, new PatrolState(this, navMeshAgent));
        states.Add(EEnemyState.Heal, new HealState());
        states.Add(EEnemyState.Chase, new ChaseState(this,navMeshAgent));
        states.Add(EEnemyState.Attack, new AttackState());
        states.Add(EEnemyState.Dead, new DeadState());

        // Set the initial state
        currentState = states[EEnemyState.Idle];
    }

    public GameObject FindClosestTarget()
    {
        // Find the closest village
        float closestDistance = Mathf.Infinity;

        if (currentNumberOfVillages > 0)
        {
            foreach (GameObject village in villages)
            {
                float distance = Vector3.Distance(transform.position, village.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closesVillage = village;
                }
            }

            return closesVillage;
        }
        else if(currentNumberOfVillages == 0)
        {
            return playerCastle;
        }
        else
        {
            return player;
        }
    }

    public void FieldOfViewCheckForTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, whatIsPlayerMask);

        if(colliders.Length != 0)
        {
            player = colliders[0].gameObject;

            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (!Physics.Raycast(transform.forward, directionToPlayer, distanceToPlayer, whatIsPlayerMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if(colliders.Length == 0)
        {
            canSeePlayer = false;
        }

    }

    public bool CheckNeedsHealingStates()
    {
        if(currentHealth < maxHealth / 2)
        {
              return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateAnimator(string animation, bool status)
    {
        animator.SetBool(animation, status);
    }



}

