using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector3 position;
    public Vector3 collisionMesh;
    public Vector3 scale;
    public GameObject flow;
    public Vector2Int offSetCoordinate;
    public Vector3Int cubeCoordinate;
    public Vector3 surviceCoordinate;

    public List<HexTile> neighbours;
    public bool hasObjects = false;
    public bool hasEnemy = false;

    public GameObject enemy;

    public void OnDrawGizmosSelected()
    {
        foreach (HexTile neighbour in neighbours)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(position, 1f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, neighbour.transform.position);
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


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hasObjects = true;

        }
        else if(collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject;
            hasObjects = true;
            hasEnemy = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hasObjects = false;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            hasObjects = false;
            hasEnemy = false;
            enemy = null;
        }
    }
}
