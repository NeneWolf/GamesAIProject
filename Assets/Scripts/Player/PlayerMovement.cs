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
    LineRenderer renderer;

    RaycastHit m_HitInfo = new RaycastHit();

    private HexTile target;
    public HexTile currentTile;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Get tile from mouse click
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo) && m_HitInfo.collider.gameObject.layer == 6)
            {
                if (m_HitInfo.collider.gameObject.TryGetComponent<HexTile>(out target))
                {
                    // Get the tile target
                    target.OnSelectTile();
                    PathFinder.FindPath(currentTile, target);
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (HexTile tile in path)
            {
                Gizmos.DrawCube(tile.transform.position + new Vector3(0, 0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f));
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