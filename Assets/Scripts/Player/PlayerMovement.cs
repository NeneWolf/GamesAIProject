using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    TileManager instance;
    Animator animator;
    NavMeshAgent m_Agent;
    NavMeshPath navMeshPath;
    LineRenderer renderer;
    PathFinder pathFinder;


    [Header("Player Stats")]
    int health = 100;
    public int currentHealth;
    public int attack;
    public float movementSpeed = 2.0f;
    bool isDead = false;

    [Header("Pathfinding")]
    RaycastHit m_HitInfo = new RaycastHit();
    public HexTile target;
    public HexTile currentTile;
    public HexTile nextTile;
    public bool hasReachedDestination;
    public bool gotPath = false;
    public bool displayPath = false;

    List<HexTile> path;
    List<HexTile> currentPath;
    List<HexTile>analised = new List<HexTile>();
    List<HexTile> notanalised = new List<HexTile>();

    [Header("Enemy")]
    public GameObject enemy;
    public bool movingToEnemy;

    [SerializeField] GameObject sword;
    SwordBehaviour swordBehaviour;

    public bool isAttacking;

    [SerializeField] LayerMask hexTileLayer;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        pathFinder = GameObject.FindAnyObjectByType<PathFinder>();
        swordBehaviour = sword.GetComponent<SwordBehaviour>();

        if (renderer == null) { return; }

        navMeshPath = GetComponent<NavMeshAgent>().path;
        instance = FindAnyObjectByType<TileManager>();
        renderer.enabled = false;

        currentHealth = health;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
        }

        if (!isDead)
        {
            HandleFindPath();

            if (gotPath)
            {
                HandleMovement();

                //TestingMovement();
            }

            //animator
            if (m_Agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            //Renderer path
            if (Input.GetKeyDown(KeyCode.Space))
            {
                displayPath = !displayPath;
                renderer.enabled = displayPath;
            }

            if (movingToEnemy && hasReachedDestination)
            {
                FindEnemy();

                if (enemy != null)
                {
                    HandleAttack();
                }
                else
                {
                    isAttacking = false;
                    animator.SetBool("isAttacking", false);
                }
            }
            else
            {
                isAttacking = false;
                if(animator.GetBool("isAttacking") == true)
                {
                    animator.SetBool("isAttacking", false);
                }
            }
        }
    }

    void HandleFindPath()
    {
        // Get tile from mouse click
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo, Mathf.Infinity, hexTileLayer) && !m_HitInfo.collider.GetComponent<HexTile>().hasObjects)
            {
                m_Agent.speed = movementSpeed;
                
                if(m_Agent.isStopped)
                {
                    m_Agent.isStopped = false;
                }

                // Get the tile target
                m_HitInfo.collider.gameObject.GetComponent<HexTile>().OnSelectTile();
                target = m_HitInfo.collider.gameObject.GetComponent<HexTile>();

                path = PathFinder.FindPath(currentTile, target);


                analised = pathFinder.GetTileListAnalised();
                notanalised = pathFinder.GetTileListNotAnalised();

                movingToEnemy = false;
                isAttacking = false;

                currentPath = path;
                gotPath = true;
                SetAgentPath(path);
            }
            else if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo, Mathf.Infinity, hexTileLayer) && m_HitInfo.collider.GetComponent<HexTile>().hasEnemy)
            {
                m_Agent.speed = movementSpeed;

                if (m_Agent.isStopped)
                {
                    m_Agent.isStopped = false;
                }

                // Get the tile target
                m_HitInfo.collider.gameObject.GetComponent<HexTile>().OnSelectTile();
                target = m_HitInfo.collider.gameObject.GetComponent<HexTile>();

                path = PathFinder.FindPath(currentTile, target);
                path.RemoveAt(path.Count - 1);

                analised = pathFinder.GetTileListAnalised();
                notanalised = pathFinder.GetTileListNotAnalised();

                movingToEnemy = true;

                currentPath = path;
                gotPath = true;
                SetAgentPath(path);
            }
            else return;
        }
    }

    void TestingMovement()
    {
        // Get tile from mouse click
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo) && m_HitInfo.collider.gameObject.layer == 6)
            {
                m_Agent.SetDestination(m_HitInfo.point);
            }
        }
    }

    void HandleMovement()
    {
        if (currentPath == null || currentPath.Count <= 1)
        {
            hasReachedDestination = true;
            m_Agent.isStopped = true;
            m_Agent.speed = 0;
            
            nextTile = null;

            if (currentTile != null && currentPath.Count > 0)
            {
                currentTile = currentPath[0];
                nextTile = currentTile;
            }

            gotPath = false;
            UpdateLineRender(new List<HexTile>());
        }
        else
        {
            hasReachedDestination = false;
            m_Agent.speed = movementSpeed;

            UpdateLineRender(currentPath);

            m_Agent.isStopped = false;

            currentTile = currentPath[0];

            nextTile = currentPath[1];

            //if (nextTile.hasObjects)
            //{
            //    currentPath.Clear();
            //    gotPath = false;
            //    path.Clear();
            //    FindPathToDestination(currentTile, target);
            //    HandleMovement();
            //    return;
            //}
            //else
            //{

            //SetAgentPath(path);

            if (currentTile != null && nextTile != null)
            {
                m_Agent.SetDestination(nextTile.transform.position);
            }

            if (m_Agent.remainingDistance < 0.1f && !m_Agent.pathPending)
            {
                currentPath.RemoveAt(0);
            }
        }
    }

    void FindEnemy()
    {
        foreach (HexTile neiTile in currentTile.neighbours)
        {
            if (neiTile.GetComponent<HexTile>().hasEnemy)
            {
                enemy = neiTile.GetComponent<HexTile>().enemy;
                return;
            }
            else {enemy = null;}
        }
    }

    void HandleAttack()
    {
        RotateToEnemy();
        isAttacking = true;
        animator.SetBool("isAttacking", true);
    }

    void RotateToEnemy()
    {
        Vector3 lookPos = enemy.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, movementSpeed);
    }

    protected void UpdateLineRender(List<HexTile> tiles)
    {
        if (renderer == null) { return; }

        List<Vector3> positions = new List<Vector3>();

        foreach (HexTile tile in tiles)
        {

            positions.Add(tile.position + new Vector3(0, (int)tile.transform.lossyScale.y / 2.5f, 0));
        }

        renderer.positionCount = positions.Count;
        renderer.SetPositions(positions.ToArray());
    }

    void FindPathToDestination(HexTile current, HexTile destination)
    {
        path = PathFinder.FindPath(currentTile, destination);
        currentPath = path;
        gotPath = true;
    }

    void SetAgentPath(List<HexTile> path)
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("Path is null or empty");
            return;
        }

        m_Agent.CalculatePath(path[path.Count - 1].position, navMeshPath);

        if (navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            m_Agent.SetPath(navMeshPath);
        }
    }

    public void ActivateSwordBehaviour()
    {
        if (enemy != null && !enemy.GetComponent<EnemyStateMachine>().reportIsDead())
        {
            enemy.GetComponent<EnemyStateMachine>().TakeDamage(attack);
        }
        else return;
    }

    public void TakeDamage(int value)
    {
        if (currentHealth - value > 0)
        {
            currentHealth -= value;
        }
        else
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void Heal(int value)
    {
        if (currentHealth + value <= health)
        {
            currentHealth += value;
        }
        else if (currentHealth + value > health)
        {
            currentHealth = health;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Heal")
        {
            collision.gameObject.GetComponent<DropBehaviour>().RetrieveValue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<HexTile>().hasObjects = true;
            other.gameObject.GetComponent<HexTile>().hasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<HexTile>().hasObjects = false;
            other.gameObject.GetComponent<HexTile>().hasPlayer = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<HexTile>().hasObjects = true;
            other.gameObject.GetComponent<HexTile>().hasPlayer = true;
        }
    }

    private void OnDrawGizmos()
    {

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

    public bool ReportIsDead()
    {
        return isDead;
    }
}