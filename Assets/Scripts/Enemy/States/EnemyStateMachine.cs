using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EEnemyState>
{
    NavMeshAgent navMeshAgent;
    NavMeshPath navMeshPath;
    PathFinder pathFinder;
    Animator animator;
    SphereCollider sphereCollider;

    public GameObject enemy;

    // States of the enemy
    [Header("EnemyStats")]
    int maxHealth = 100;
    [SerializeField] int currentHealth;
    [SerializeField] int damage = 10;
    bool isDead;
    [SerializeField] int speed;
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

    [Header("Healing & Call for help")]
    public GameObject nearestHeal;
    public bool isThereHealing;

    public GameObject[] allNearbyEnemies;
    public bool isBeingCalledForHelp;

    //Pathfinding
    [Header("Pathfinding")]
    RaycastHit m_hitInfo = new RaycastHit();
    public HexTile targetTile;
    public HexTile currentTile;
    public HexTile nextTile;
    public bool hasReachedDestination;
    public bool gotPath = false;
    public bool displayPath = false;

    List<HexTile> path;
    List<HexTile> currentPath;

    //View Gizmos Pathfinding
    List<HexTile> analised = new List<HexTile>();
    List<HexTile> notanalised = new List<HexTile>();


    public bool movingToAttack;

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
        navMeshPath = GetComponent<NavMeshAgent>().path;
        pathFinder = GameObject.FindAnyObjectByType<PathFinder>();

        animator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radiusToRunAway;
        enemy = this.gameObject;

        AddStates();

        // Initialize the enemy's health
        currentHealth = maxHealth;
        navMeshAgent.speed = speed;

        // Temporary
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


        if (gotPath)
        {
            HandleEnemyMovement();
        }
    }

    void AddStates()
    {
        // Initialize your enemy states and add them to the 'states' dictionary
        states.Add(EEnemyState.Idle, new IdleState(this,waiting));
        states.Add(EEnemyState.Patrol, new PatrolState(this, navMeshAgent));
        states.Add(EEnemyState.Heal, new HealState(this, navMeshAgent));
        states.Add(EEnemyState.Chase, new ChaseState(this,navMeshAgent));
        states.Add(EEnemyState.AttackMelee, new AttackMelee(this, navMeshAgent));
        states.Add(EEnemyState.AttackRanged, new AttackRanged(this));
        states.Add(EEnemyState.Dead, new DeadState());

        // Set the initial state
        currentState = states[EEnemyState.Idle];
        currentState.EnterState();
    }

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
        currentHealth -= damage;
       // isBeingAttacked = true;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }



    public bool CheckNeedsHealingStates()
    {
        if(currentHealth < maxHealth / 2)
        {
              FindTheNearestHealingTile();
              if (navMeshAgent.speed == speed) { navMeshAgent.speed /= 2; }
              return true;
        }
        else
        {
            navMeshAgent.speed = speed;
            return false;
        }
    }



    public void FindTheNearestHealingTile()
    {
        GameObject[] healing = GameObject.FindGameObjectsWithTag("Heal");

        if (healing.Length > 0)
        {
            isThereHealing = true;
            GameObject closestHeal = null;
            closestHeal = healing[0];

            foreach (GameObject h in healing)
            {
                float distance = Vector3.Distance(transform.position, h.transform.position);
                float distance2 = Vector3.Distance(transform.position, closestHeal.transform.position);

                if(distance < distance2)
                {
                    closestHeal = h;
                }
            }

            target = closestHeal;
        }
        else
        {
            target = null;
            isThereHealing = false;
        }

    }

    public void FindAllEnemiesAndCallForHelp()
    {
        allNearbyEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in allNearbyEnemies)
        {
            if (Vector3.Distance(this.transform.position,enemy.transform.position) < 15f)
            {
                enemy.gameObject.GetComponent<EnemyStateMachine>().isBeingCalledForHelp = true;
                enemy.gameObject.GetComponent<EnemyStateMachine>().target = target;
                enemy.gameObject.GetComponent<EnemyStateMachine>().currentState = states[EEnemyState.Chase];
                enemy.gameObject.GetComponent<EnemyStateMachine>().currentState.EnterState();
            }
        }
    }



    public void Heal(int value)
    {
        currentHealth += value;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }   
    }




    public void UpdateAnimator(string animation, bool status)
    {
        animator.SetBool(animation, status);
    }



    //Mainly For Idle State
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
        if(currentState.stateKey == EEnemyState.Chase || currentState.stateKey == EEnemyState.Heal)
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


        if (displayPath && gotPath)
        {
            foreach (HexTile tile in analised)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(tile.position, 1f);
            }

            foreach (HexTile tile in notanalised)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(tile.position, 1f);
            }

            foreach (HexTile tile in path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(tile.position, 1f);
            }
        }
    }

    public bool reportIsDead()
    {
        return isDead;
    }


    void HandleEnemyMovement()
    {
        if (currentPath == null || currentPath.Count <= 1)
        {
            hasReachedDestination = true;
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;

            nextTile = null;

            if (currentTile != null && currentPath.Count > 0)
            {
                currentTile = currentPath[0];
                nextTile = currentTile;
            }

            gotPath = false;
        }
        else
        {
            hasReachedDestination = false;
            navMeshAgent.speed = speed;

            navMeshAgent.isStopped = false;

            currentTile = currentPath[0];

            nextTile = currentPath[1];

            if (currentTile != null && nextTile != null)
            {
                navMeshAgent.SetDestination(nextTile.transform.position);
            }

            if (navMeshAgent.remainingDistance < 0.1f && !navMeshAgent.pathPending)
            {
                currentPath.RemoveAt(0);
            }
        }
    }

    public void FindPathToRandomPoint()
    {
        targetTile = RandomNavmeshLocation();
        path = PathFinder.FindPath(currentTile, targetTile);

        movingToAttack = false;

        analised = pathFinder.GetTileListAnalised();
        notanalised = pathFinder.GetTileListNotAnalised();

        currentPath = path;
        gotPath = true;
        SetAgentPath(path);
    }

    public void FindPathToTarget()
    {
        if(target != null)
        {
            if(target.CompareTag("Player"))
            {
                targetTile = target.GetComponent<PlayerMovement>().currentTile;
            }
            else if(target.CompareTag("Village") || target.CompareTag("PlayerCastle"))
            {
                targetTile = target.GetComponent<DetailMovement>().currentTile;
            }

            path = PathFinder.FindPath(currentTile, targetTile);
            path.RemoveAt(path.Count - 1);

            movingToAttack = true;

            analised = pathFinder.GetTileListAnalised();
            notanalised = pathFinder.GetTileListNotAnalised();

            currentPath = path;
            gotPath = true;
            SetAgentPath(path);

        }
    }

    public HexTile RandomNavmeshLocation()
    {
        List<HexTile> hexTiles = HexTilesWithinRadius(currentTile, patrolRadius);

        if (hexTiles.Count > 0)
        {
            // Pick a random index
            int randomIndex = Random.Range(0, hexTiles.Count);

            // Return the randomly selected HexTile
            return hexTiles[randomIndex];
        }

        return null;
    }

    public List<HexTile> HexTilesWithinRadius(HexTile currentTile, float radius)
    {
        List<HexTile> hexTiles = new List<HexTile>();

        Collider[] colliders = Physics.OverlapSphere(currentTile.transform.position, radius);

        foreach (var collider in colliders)
        {
            if(collider.gameObject.layer == 6)
            {
                HexTile hexTile = collider.GetComponent<HexTile>();

                if (hexTile != null &&
                    !hexTile.GetComponent<HexTile>().hasObjects ||
                    !hexTile.GetComponent<HexTile>().hasEnemy ||
                    !hexTile.GetComponent<HexTile>().hasEnemy)
                {
                    hexTiles.Add(hexTile);
                }
            }
        }

        return hexTiles;
    }

    void SetAgentPath(List<HexTile> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("Path is null or empty");
            return;
        }

        navMeshAgent.CalculatePath(path[path.Count - 1].position, navMeshPath);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetPath(navMeshPath);
        }
    }

    public bool FindPlayer()
    {
        foreach (HexTile neiTile in currentTile.neighbours)
        {
            if (neiTile.GetComponent<HexTile>().hasPlayer)
            {
                return true;
            }
            else { return false; }
        }

        return false;
    }

    public void RotateToTarget()
    {
        Vector3 lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
    }

}

