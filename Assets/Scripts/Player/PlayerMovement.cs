using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    TileManager instance;

    List<HexTile> path;
    List<HexTile> currentPath;

    NavMeshAgent m_Agent;
    NavMeshPath navMeshPath;

    LineRenderer renderer;

    RaycastHit m_HitInfo = new RaycastHit();

    public HexTile target;
    public HexTile currentTile;
    public HexTile nextTile;

    float movementSpeed = 2.0f;

    public bool gotPath = false;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<LineRenderer>();    

        if (renderer == null) { return; }

        navMeshPath = GetComponent<NavMeshAgent>().path;
        instance = FindAnyObjectByType<TileManager>();
    }

    void Update()
    {
        HandleFindPath();
        
        if(gotPath)
        {
            HandleMovement();

            //TestingMovement();
        }
    }

    void HandleFindPath()
    {
        // Get tile from mouse click
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo) && m_HitInfo.collider.gameObject.layer == 6)
            {
                // Get the tile target
                m_HitInfo.collider.gameObject.GetComponent<HexTile>().OnSelectTile();
                target = m_HitInfo.collider.gameObject.GetComponent<HexTile>();

                path = PathFinder.FindPath(currentTile, target);
                currentPath = path;
                gotPath = true;
                //SetAgentPath(path);
            }
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
        if(currentPath == null || currentPath.Count <= 1)
        {
            nextTile = null;

            if(currentTile != null && currentPath.Count > 0)
            {
                currentTile = currentPath[0];
                nextTile = currentTile;

            }

            gotPath = false;
            UpdateLineRender(new List<HexTile>());
        }
        else
        {
            currentTile = currentPath[0];

            nextTile = currentPath[1];

            if (nextTile.hasObjects)
            {
                currentPath.Clear();
                HandleMovement();
                return;
            }

            // Calculate the time-based interpolation factor
            float t = Time.deltaTime * movementSpeed;

            // Interpolate the player's position
            this.transform.position = Vector3.Lerp(this.transform.position, nextTile.transform.position + new Vector3(0, 0.5f, 0), t);


            // Get the positions of the objects
            Vector3 positionA = transform.position;
            Vector3 positionB = nextTile.position;

            // Calculate the distance on the X and Z axes (ignoring Y-axis)
            float distanceX = Mathf.Abs(positionA.x - positionB.x);
            float distanceZ = Mathf.Abs(positionA.z - positionB.z);

            // Calculate the total distance
            float totalDistance = Mathf.Sqrt(distanceX * distanceX + distanceZ * distanceZ);


            // Check if the player has reached the next tile
            if (totalDistance < 0.01f)
            {
                currentPath.RemoveAt(0);
                instance.playerPos = nextTile.cubeCoordinate;
                UpdateLineRender(currentPath);
            }
        }
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

    protected void UpdateLineRender(List<HexTile> tiles)
    {
        if (renderer == null) { return; }

        List<Vector3> positions = new List<Vector3>();

        foreach (HexTile tile in tiles)
        {
            positions.Add(tile.position + new Vector3(0, 0.5f, 0));
        }

        renderer.positionCount = positions.Count;
        renderer.SetPositions(positions.ToArray());
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (HexTile tile in path)
            {
                Debug.Log("Drawing path");
                Gizmos.DrawCube(tile.transform.position + new Vector3(0, 10f, 0), new Vector3(2, 2, 2));
            }
        }
    }
}