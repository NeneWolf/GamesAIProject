using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    EnemyPathFinding pathFinding;
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        pathFinding = GameObject.FindFirstObjectByType<MapGenerator>().enemyPathFindingGrid;


        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo) && m_HitInfo.collider.gameObject.layer == 6)
            {
                #region
                //if (pathFinding != null)
                //{
                //    Vector3 worldPosition = m_HitInfo.point;
                //    pathFinding.GetGrid().GetXZ(worldPosition, out int x, out int z);
                //    List<PathNode> path = pathFinding.Findpath(0, 0, x, z);
                //    if (path != null)
                //    {
                //        for (int i = 0; i < path.Count - 1; i++)
                //            Debug.DrawLine(new Vector3(path[i].x, 0, path[i].z) * ((2.9f * 2f) - 0.8f), new Vector3(path[i + 1].x, 0, path[i + 1].z) * ((2.9f * 2f) - 0.8f), Color.red, 100f);

                //    }
                //}
                #endregion
                m_Agent.destination = m_HitInfo.point;
            }
        }
    }
}