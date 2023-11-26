using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    GameManager gameManager;

    public Vector3 position;
    public Vector3 collisionMesh;
    public Vector3 scale;
    public GameObject flow;
    public Vector2Int offSetCoordinate;
    public Vector3Int cubeCoordinate;
    public Vector3 surviceCoordinate;
    public float heightWeight;

    public List<HexTile> neighbours;
    public GameObject DecorInHexigon;
    public bool hasBeenRequestedToClearNeightbours;

    public bool hasObjects = false;
    public bool isImportantBuilding = false;
    public bool hasEnemy = false;
    public bool hasPlayer = false;

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

    private void Awake()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        if (hasBeenRequestedToClearNeightbours && neighbours != null)
        {
            DestroyNeighboursDecor();
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

    void DestroyNeighboursDecor()
    {
        if (!gameManager.hasGameStarted) 
        {
            foreach (HexTile neighbour in neighbours)
            {
                if (neighbour.hasObjects && !neighbour.DecorInHexigon.GetComponent<DetailMovement>().ReportImportancy())
                {
                    DestroyImmediate(neighbour.GetComponent<HexTile>().DecorInHexigon);
                    neighbour.hasObjects = false;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasObjects = true;
            hasPlayer = true;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.GetComponentInParent<EnemyStateMachine>().gameObject;
            hasObjects = true;
            hasEnemy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasObjects = false;
            hasPlayer = false;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            enemy = null;
            hasObjects = false;
            hasEnemy = false;
        }
        else if (other.gameObject.tag == "Village" || other.gameObject.tag == "PlayerCastle")
        {
            hasObjects = false;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        hasObjects = true;

    //    }
    //    else if(collision.gameObject.tag == "Enemy")
    //    {
    //        enemy = collision.gameObject;
    //        hasObjects = true;
    //        hasEnemy = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        hasObjects = false;
    //    }
    //    else if (collision.gameObject.tag == "Enemy")
    //    {
    //        hasObjects = false;
    //        hasEnemy = false;
    //        enemy = null;
    //    }
    //}
}
