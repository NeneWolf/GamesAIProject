using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EEnemyState>
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    SphereCollider sphereCollider;

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
    public GameObject[] villages;
    public GameObject closesVillage;

    public bool isPlayerCastleAlive = true;
    GameObject playerCastle;

    public bool isPlayerAlive = true;
    GameObject player;
    public GameObject target;

    public int patrolRadius = 10;   

    [Header("FieldOfView")]
    [Space(10)]
    public bool canSeeTarget = false;
    public LayerMask whatIsTargetMask;

    [Space(10)]
    public float radiusToCheck;
    public float radiusToRunAway;
    [Range(0, 360)]
    public float angle;

    EEnemyState nextStateKey;
    public EEnemyState currentStateDisplayTEST;

    public bool waiting = true;

    public bool isBeingAttacked;

    // Enum of the enemy states
    public enum EEnemyState
    {
        Idle,
        Patrol,
        Heal,
        Chase,
        AttackMelee,
        AttackRanged,
        Dead
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radiusToRunAway;
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

        // Initialize the target castle
        playerCastle = GameObject.FindGameObjectWithTag("PlayerCastle");

        // Initialize the target
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void Update()
    {
        currentStateDisplayTEST = currentState.stateKey;
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
        states.Add(EEnemyState.Idle, new IdleState(this,waiting));
        states.Add(EEnemyState.Patrol, new PatrolState(this, navMeshAgent));
        states.Add(EEnemyState.Heal, new HealState());
        states.Add(EEnemyState.Chase, new ChaseState(this,navMeshAgent));
        states.Add(EEnemyState.AttackMelee, new AttackState());
        states.Add(EEnemyState.AttackRanged, new AttackRanged(this));
        states.Add(EEnemyState.Dead, new DeadState());

        // Set the initial state
        currentState = states[EEnemyState.Idle];
        currentState.EnterState();
    }

    //public GameObject FindClosestTarget()
    //{
    //    // Find the closest village
    //    float closestDistance = Mathf.Infinity;

    //    if (currentNumberOfVillages > 0)
    //    {
    //        foreach (GameObject village in villages)
    //        {
    //            float distance = Vector3.Distance(transform.position, village.transform.position);

    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                closesVillage = village;
    //            }
    //        }

    //        return closesVillage;
    //    }
    //    else if(currentNumberOfVillages == 0)
    //    {
    //        return playerCastle;
    //    }
    //    else
    //    {
    //        return target;
    //    }
    //}

    public void FieldOfViewCheckForTargets()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusToCheck, whatIsTargetMask);

        if (colliders.Length >= 1)
        {
            foreach (Collider col in colliders)
            {
                Vector3 directionToPlayer = (col.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToPlayer) < angle / 2)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, col.transform.position);

                    if (!Physics.Raycast(transform.forward, directionToPlayer, distanceToPlayer, whatIsTargetMask))
                    {
                        if (col.gameObject.tag == "Player" && currentNumberOfVillages == 0 && !isPlayerCastleAlive)
                            target = col.gameObject;
                        else if (currentNumberOfVillages > 0 && col.gameObject.tag == "Village")
                            target = col.gameObject;
                        else if (currentNumberOfVillages == 0 && isPlayerCastleAlive)
                            target = playerCastle;
                        else if(col.gameObject.tag == "Player" && colliders.Length == 2)
                            target = col.gameObject;

                        canSeeTarget = true;
                    }
                    else
                    {
                        canSeeTarget = false;
                        target = null;
                    }
                }
                else if (target == null)
                {
                    canSeeTarget = false;
                    return;
                }
            }
        }
        else if (colliders.Length == 0)
        {
            canSeeTarget = false;
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        isBeingAttacked = true;
        target = player;
        currentHealth -= damage;
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

    public void WaitForTime(int time)
    {
        StartCoroutine(Wait(time));
    }

    IEnumerator Wait(int amount)
    {
        waiting = true;
        yield return new WaitForSeconds(amount);
        waiting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(currentState.stateKey == EEnemyState.Chase)
        {
            currentState.OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentState.stateKey == EEnemyState.Chase)
        {
            currentState.OnTriggerExit(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentState.stateKey == EEnemyState.Chase)
        {
            currentState.OnTriggerStay(other);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2);
        Gizmos.DrawWireSphere(transform.position, 6);
    }
}

