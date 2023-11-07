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
    List<HexTile> path;

    NavMeshAgent m_Agent;
    NavMeshPath navMeshPath;
    LineRenderer renderer;

    RaycastHit m_HitInfo = new RaycastHit();

    public HexTile target;
    public HexTile currentTile;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //TestingMovement();

        Movement();
    }

    void Movement()
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
                SetAgentPath(path);
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

    public void OnDrawGizmos()
    {
        // Debug.Log("Drawing gizmos");
        if (path != null)
        {
            foreach (HexTile tile in path)
            {
                Debug.Log("Drawing path");
                Debug.Log(tile.transform.name);
                Gizmos.DrawCube(tile.transform.position + new Vector3(0, 20f, 0), new Vector3(2, 2, 2));
            }
        }
    }

    //protected void UpdateLineRender(List<HexTile> tiles)
    //{
    //    if(renderer = null) { return; }

    //    List<Vector3> positions = new List<Vector3>();
    //    foreach(HexTile tile in tiles)
    //    {
    //        points.Add(tile.transform.position + new Vector3(0,0.5f,0));
    //    }

    //    renderer.positionCount = positions.Count;
    //    renderer.SetPositions(positions.ToArray());
    //}
}