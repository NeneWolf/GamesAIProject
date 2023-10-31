using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector3 position;
    public Vector3 scale;
    public GameObject flow;
    public Vector2Int offSetCoordinate;
    public Vector3Int cubeCoordinate;
    public List<HexTile> neighbors;

    public void OnDrawGizmosSelected()
    {
        foreach (HexTile neighbor in neighbors)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(position, 1f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    public void OnHighlightTile()
    {
        TileManager tilemanager = GameObject.FindFirstObjectByType<TileManager>();
        tilemanager.OnHighlightTile(this);
    }

    public void OnSelectTile()
    {
        TileManager tilemanager = GameObject.FindFirstObjectByType<TileManager>();
        tilemanager.OnSelectTile(this);
    }

    //public bool isWalkable = false;
}
